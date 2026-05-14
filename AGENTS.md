# Hospitaly — Agent Guide
## Clean Architecture Rule: DTOs, Commands, and Domain Value Objects

When implementing features, keep transport models, use-case commands, and domain objects separated.

### Rule

- API/Controller request DTOs should use request-friendly primitive types such as `string`, `int`, `Guid`, `DateTime`, etc.
- Application commands should usually use primitive or lightweight use-case input types.
- Domain value objects and domain-specific types must be created inside the application use case/command handler, not directly inside the controller.
- Do not expose domain value objects such as `Sex`, `BloodType`, `Email`, `PhoneNumber`, etc. directly through API request DTOs.
- Do not create duplicate domain-like objects in the application layer unless there is a strong reason.

### Preferred Flow

```text
HTTP Request DTO
    ↓
Application Command
    ↓
Command Handler creates/validates Domain Value Objects
    ↓
Domain Entity / Aggregate
## Project overview
Modular monolith hospital management system. .NET 10, Clean Architecture (Domain → Application → Infrastructure → Presentation per module), Angular 21 SPA.

## Solution structure
- `Hospitaly.slnx` (new `.slnx` format — open with `dotnet slnx` or Rider)
- 2 API hosts: `Hospitaly.Api` (backend API) + `Hospitaly.Bff` (YARP reverse proxy + OIDC auth)
- Common libs: `Hospitaly.Common.{Domain,Application,Infrastructure,Presentation}`
- Modules (each with same 4-layer split): `Cliniks`, `Users`
- Tests: `tests/ArchitectureTests/` (root level), `src/Modules/Cliniks/tests/` (module-level)

## Commands
```powershell
dotnet build Hospitaly.slnx
dotnet test Hospitaly.slnx
dotnet test tests/ArchitectureTests
dotnet test src/Modules/Cliniks/tests/Hospitaly.Modules.Cliniks.ArchitectureTests
```
Angular (in `src/Api/Hospitaly.Bff/Hospitaly.Client/`): `ng serve`, `ng build`, `ng test`

## Stack & quirks
- **Auth**: Keycloak OIDC (BFF initiates flow, stores session tokens in Redis, YARP attaches Bearer token upstream). Two valid issuers configured (localhost + Docker hostname).
- **DB**: PostgreSQL via Npgsql. EF Core for commands/writes, Dapper for queries. Migrations auto-applied in `Development` via `ApplyMigrations()`.
- **Cache/Sessions**: Redis (password: `hospitaly123`). BFF uses Redis-backed `SessionService` for token storage and auto-refresh.
- **Config loading**: API uses module-scoped JSON files at `appSettings/{module}/modules.{module}.json` + `.Development.json`, loaded via `AddModuleConfiguration(["Users"])`. Only Users module config is wired.
- **Route prefix**: `api/` (API) or `bff/` (BFF) via `GlobalRoutePrefixConvention`.
- **Docker**: 6 services (bff, api, postgres, keycloak, redis, redis-insight). Run via `docker-compose up`.

## Architecture conventions (enforced by tests)
- Commands sealed, non-public, name ends with `Command`
- CommandHandlers sealed, not public
- Domain events sealed, name ends with `DomainEvent`
- Entities must have private parameterless constructor (EF Core requirement)
- ValueObjects used for domain primitives (PhoneNumber, Email, Address, etc.)

## Key DI wiring
- `AddInfrastructure(connectionString)` → JWT auth, Npgsql data source, permission-based auth (custom `IPermissionsService`)
- `AddModules(configuration)` → MediatR (CQRS), module-specific infra
- `AddApplicationServices(assemblies[])` → MediatR registration

## Tests
- xUnit + FluentAssertions + NetArchTest.Rules + Coverlet
- Module-level architecture tests verify layer dependencies and naming conventions
- Root `tests/ArchitectureTests/` is scaffold only (empty `BaseTest`)

## Angular client
- Built with Angular CLI 21, optional Angular dev server at `localhost:4200`
- Vitest (not Karma) for unit tests via `ng test`
- Tailwind CSS v4 + PostCSS
- Previously had a separate `AGENTS.md`/`agents.md` in that subdirectory — most of its content was generic (not repo-specific). The `src/Api/Hospitaly.Bff/Hospitaly.Client/` subtree has its own guidance but is scoped to client-only work.

## Authentication and Authorization Flow

### Architecture overview
This project uses a **BFF (Backend-for-Frontend) security pattern** with OIDC / Keycloak. The Angular SPA never handles tokens directly. All authentication is managed server-side by `Hospitaly.Bff` (YARP reverse proxy + OIDC middleware), which issues an HttpOnly session cookie to the browser and stores tokens in Redis.

### Complete auth flow

```
Angular SPA (localhost:4200)
  │
  ├─ Login button → window.location = /bff/auth/login?returnUrl=...
  │
  ▼
BFF (localhost:7214)
  ├─ GET /bff/auth/login → Challenge(OpenIdConnect)
  │   └─ Redirects to Keycloak (localhost:28080/realms/hospitaly)
  │
  ▼
Keycloak
  ├─ User authenticates
  └─ Redirects back to /signin-oidc with auth code
  │
  ▼
BFF — OnTokenValidated event:
  ├─ Generates session_id (GUID)
  ├─ Stores access_token, refresh_token, expiry in Redis
  │   Key: session:{sessionId}, TTL: 7 days
  ├─ Adds session_id claim to the principal
  ├─ Sets HttpOnly auth cookie (ASP.NET Core Cookie defaults)
  └─ Redirects to returnUrl (default: https://localhost:4200/)
  │
  ▼
Angular boots — APP_INITIALIZER fires:
  ├─ AuthService.checkSession()
  │   ├─ GET /bff/auth/check_session → validates cookie → returns OIDC claims
  │   └─ GET /bff/user/me
  │       ├─ BFF reads Redis session by session_id claim
  │       ├─ BFF checks Redis cache at client_user_data:{userId} (TTL: 15min)
  │       ├─ Cache miss → BFF calls GET /api/users/me via YARP with Bearer token
  │       │   └─ YARP transform: extracts session_id → Redis session → attaches Bearer
  │       ├─ API validates JWT (Keycloak issuer)
  │       ├─ API runs CustomClaimsTransformation → loads permissions from DB
  │       └─ Returns ClientUserData { userId, userName, email, roles, permissions, requiresOnboarding }
  └─ If profile.requiresOnboarding && url !== '/onboarding'
      └─ router.navigateByUrl('/onboarding')
  │
  ▼
Protected routes (authGuard):
  ├─ Checks AuthService.isAuthenticated() signal
  ├─ If false → calls AuthService.checkSession()
  └─ If session invalid → calls AuthService.login() → redirects to BFF
```

### Token and session management

- **Angular must not directly manage access tokens.** The BFF pattern prohibits accessing tokens on the client. The `credentialsInterceptor` sets `{ withCredentials: true }` on all HTTP requests, sending the session cookie automatically.
- **Redis** (hosted at `localhost:6379`, password `hospitaly123`) stores three key types:
  - `session:{sessionId}` — UserSession JSON (includes access/refresh tokens), 7-day TTL
  - `user_sessions:{userId}` — Redis Set of session IDs per user (no TTL)
  - `client_user_data:{userId}` — Cached user profile, 15-minute TTL
- **Token refresh**: Handled automatically by the BFF. Before proxying any `api/**` request, YARP checks if the token expires within 30 seconds and refreshes via Keycloak's token endpoint if needed. Angular is never involved.

### YARP reverse proxy routing

BFF is at `https://localhost:7214`. YARP routes:
- `api/{**catch-all}` → proxied to `http://localhost:5500/` with Bearer token attached
- Everything else (`bff/auth/*`, `bff/user/*`, `bff/sessions/*`) → handled locally by BFF controllers

Never call `http://localhost:5500` (the API) directly from Angular. All API calls must go through `https://localhost:7214/api/...` so YARP can attach the session token.

### Strict Angular → BFF → API flow (required)

Use this rule for all normal business features (clinics, users, appointments, etc.).

- Angular must always send backend/API requests to the BFF host.
- The BFF uses YARP reverse proxy to forward API requests to the backend API.
- Do not make Angular call the API directly.
- Do not create BFF controller endpoints just to manually handle normal API operations.
- Do not create BFF endpoints like "update clinic info", "create clinic", "update user profile", etc. if the API already owns that operation.
- For normal business operations, Angular should call the BFF route and YARP should forward it to the API.
- BFF controllers should only be used for true BFF-specific behavior (login, logout, session management, auth callbacks, `/me`, or frontend-specific composition).
- Do not duplicate API DTOs inside the BFF unless there is a real BFF-specific reason.
- Do not move business/domain logic into the BFF.

Correct example:

```text
Angular sends:
PUT /bff/api/clinics/{clinicId}

BFF/YARP forwards to:
PUT /api/clinics/{clinicId}

API handles validation, command/query dispatch, business logic, database update, and response.
```

Incorrect example (avoid):

```text
Angular sends:
PUT /bff/clinic/update-info

BFF controller receives a DTO, manually calls the API, and maps the result.
```

This incorrect style should not be used unless there is a very specific BFF-only reason.

Angular response handling rule:

- Angular services should unwrap `ApiResponse<T>` and return the actual `data` model to components.
- Components should not repeatedly deal with `ApiResponse<T>` unless there is a specific reason.

Example:

```ts
// service
updateClinic(clinicId: string, payload: UpdateClinicRequest): Observable<ClinicDto> {
  return this.http
    .put<ApiResponse<ClinicDto>>(`/bff/api/clinics/${clinicId}`, payload)
    .pipe(map((response) => response.data));
}

// component
save(): void {
  this.clinicsService.updateClinic(this.clinicId, this.form.value).subscribe((clinic) => {
    this.clinic = clinic;
  });
}
```

### `/me` endpoint and user state

- `GET /bff/user/me` is the single source of truth for frontend user state.
- Called during `AuthService.checkSession()` which runs at app startup (`APP_INITIALIZER`) and when `authGuard` validates a stale session.
- Returns `ClientUserData` with `roles`, `permissions`, and `requiresOnboarding`.
- Angular stores this in `AuthService.profile` signal — the reactive source for all user data.

### Authorization decisions

**Backend (API):** The API enforces authorization via:
- `[Authorize]` — any authenticated user
- `[Authorize("permission:name")]` — dynamic policy checked by `PermissionAuthorizationHandler` against DB-stored permissions loaded via `CustomClaimsTransformation`
- `HttpContext.User.GetUserId()` or `GetPermissions()` extension methods

**Frontend (Angular):** Current state — roles and permissions from `/me` are displayed on the profile page but **no frontend permission-checking infrastructure exists** (no `hasPermission` directive/pipe/service). When adding UI-level authorization, use `AuthService.profile()?.permissions` or create a dedicated authorization service. Do not hardcode role/permission logic in components.

### Onboarding flow

- The `requiresOnboarding` flag in `ClientUserData` controls whether the onboarding wizard is shown.
- After app initialization, if `requiresOnboarding === true` and the user is not on `/onboarding`, they are redirected automatically.
- The onboarding wizard (`OnboardingPage`) offers role selection (Doctor, Clinic Owner, or None) and calls the appropriate API.
- On completion, `POST /bff/user/onboarding/complete` marks onboarding done server-side and invalidates the Redis user data cache.
- A fresh `/bff/user/me` call updates `requiresOnboarding` to `false`.

### Logout

- Angular calls `AuthService.logout()` which:
  1. Clears local signals (`user`, `profile`, `isAuthenticated` set to null/false)
  2. Sets `window.location.href = 'https://localhost:7214/bff/auth/logout'`
- BFF `GET /bff/auth/logout`:
  1. Revokes ALL Redis sessions for the user via `SessionService.RevokeAllSessionsAsync`
  2. Signs out of Cookie + OIDC schemes
  3. Redirects to `https://localhost:4200/`
- Keycloak's end-session endpoint is called as part of the OIDC sign-out (uses `SignedOutCallbackPath: /signout-callback-oidc`).
- The redirect URI (`https://localhost:4200/`) must be registered as a valid redirect URI in the Keycloak client.

### Keycloak clients

| Client | Grant Type | Purpose |
|---|---|---|
| `hospitaly-bff-client` | Authorization code + PKCE | BFF OIDC login flow |
| `hospitaly-confidential-api` | Client credentials | Keycloak Admin REST API (user registration) |

Two valid issuers: `http://localhost:28080/realms/hospitaly` and `http://hospitaly.identity:8080/realms/hospitaly`.

### Implementation rules for future agents

1. **Do not bypass the BFF.** Never call the API directly from Angular. Always go through `https://localhost:7214/api/...`.
2. **Do not store access tokens** in `localStorage`, `sessionStorage`, or any Angular state. The BFF pattern manages tokens server-side.
3. **Always preserve route guards and interceptors.** The `authGuard` and `credentialsInterceptor` are essential for auth to work.
4. **Always update tests or add notes** when modifying auth behavior. Architecture tests enforce layer separation.
5. **Verify Keycloak redirect URIs** when changing login/logout routes. Both the BFF config and Keycloak client config must agree.
6. **Keep authentication and authorization concerns separated.** AuthN = session validation (BFF), AuthZ = permission checks (API via `IPermissionsService`).
7. **When adding protected pages**, use the existing `authGuard` in the route definition.
8. **When adding role/permission checks**, either use the existing `AuthService.profile()?.permissions` or create a dedicated authorization service. Do not hardcode logic in component templates.
9. **Auth URLs are currently hardcoded** in Angular services (`https://localhost:7214`). If adding environment configuration, ensure all services are updated consistently.
10. **The onboarding flag is the only first-time-login mechanism.** It persists until `POST /bff/user/onboarding/complete` is called. If the user leaves onboarding incomplete, the flag remains true and the wizard reappears next login.
