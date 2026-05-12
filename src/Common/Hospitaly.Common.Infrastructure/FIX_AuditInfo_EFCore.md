# Fix: `AuditInfo` treated as an EF Core entity

## Problem

At startup, EF Core can throw:

`System.InvalidOperationException: The entity type 'AuditInfo' requires a primary key to be defined...`

This happens because `AuditInfo` is exposed as a reference property (`Audit`) on the shared base `Entity`, and EF tries to map it as a normal entity unless configured.

## Why this happens

- `Entity` contains: `public AuditInfo Audit { get; private set; }`
- Domain entities inherit from `Entity` / `AggregateRoot`
- EF model discovery sees `Audit` and includes `AuditInfo` in the model
- `AuditInfo` has no key, so EF fails model validation

## Fix options

Choose one option per module/entity depending on your persistence intent.

### Option A: Persist audit fields in the owner table (recommended when audit is needed in DB)

Configure `Audit` as an owned/complex value object.

Example (`IEntityTypeConfiguration<T>`):

```csharp
builder.OwnsOne(x => x.Audit, audit =>
{
    audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy");
    audit.Property(a => a.CreatedOnUtc).HasColumnName("CreatedOnUtc");
    audit.Property(a => a.UpdatedBy).HasColumnName("UpdatedBy");
    audit.Property(a => a.UpdatedOnUtc).HasColumnName("UpdatedOnUtc");
});
```

Notes:
- This stores audit columns in the same table as the owning entity.
- `AuditInfo` is treated as a value object, not a standalone entity.

### Option B: Do not persist audit for that entity/module

Ignore the inherited `Audit` property.

```csharp
builder.Ignore(x => x.Audit);
```

In this repository, Users module currently uses this approach for `User`:
- `src/Modules/Users/Hospitaly.Modules.Users.Infrastructure/Database/Configurations/UserConfiguration.cs`

## Which option to choose

- Use **Option A** when audit fields must be saved and queried from the database.
- Use **Option B** when audit is domain-only for that module/entity.

## Quick recovery checklist

1. Find the failing entity/context from the exception.
2. Check if the entity inherits from `Entity` and has `Audit`.
3. Decide intent:
   - persist audit -> configure `OwnsOne` (or `ComplexProperty` if preferred)
   - do not persist audit -> `Ignore(x => x.Audit)`
4. Rebuild and run.

## Verify

```powershell
dotnet build Hospitaly.slnx
dotnet test Hospitaly.slnx
```
