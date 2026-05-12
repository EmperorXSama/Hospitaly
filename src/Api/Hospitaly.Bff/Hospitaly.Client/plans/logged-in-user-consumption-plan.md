# Plan: Consume Logged-In User Endpoint from Angular via BFF

## Goal
Consume the logged-in user endpoint from Angular (`Hospitaly.Client`) by calling only BFF APIs, then expose and render user data (name/email/roles/permissions) for authenticated users.

## Current Architecture
- BFF uses cookie + OIDC authentication, stores session tokens in Redis, and issues a `session_id` claim.
- YARP forwards authenticated requests to backend API by attaching Bearer access token from Redis session.
- BFF exposes `GET /bff/user/me` that returns cached `ClientUserData`.
- Angular already talks to BFF for `check_session`, `login`, and `logout` with credentials.

## Implementation Tasks
1. Add frontend model for the user data payload.
2. Add Angular service to call `GET /bff/user/me`.
3. Extend `AuthService` to load and hold profile data after successful session check.
4. Update navbar to display profile name from loaded user data.
5. Keep fallback to claim-based name rendering for resilience.
6. Verify by running Angular build/tests and checking network calls.

## Validation Checklist
- Logging in from SPA ends with authenticated state.
- Angular calls `GET /bff/user/me` (not direct backend API).
- Navbar shows profile user name.
- Logout clears both auth claims and profile state.
- Expired session falls back to unauthenticated state.
