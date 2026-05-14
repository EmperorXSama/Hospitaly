using Hospitaly.Api.Extensions;
using Hospitaly.Common.Infrastructure;
using Hospitaly.Common.Infrastructure.Seeder;
using Hospitaly.Common.Presentation;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

string databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;


builder.Configuration.AddModuleConfiguration(["Users"]);
builder.Services.AddControllers(options =>
    {
        options.Conventions.Add(new GlobalRoutePrefixConvention("api"));
    })
    .AddApplicationPart(Hospitaly.Modules.Users.Presentation.ReferenceAssembly.assembly)
    .AddApplicationPart(Hospitaly.Modules.Clinic.Presentation.AssemblyReference.assembly);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Components ??= new OpenApiComponents();

        var bearerScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token"
        };

        document.AddComponent("Bearer", bearerScheme);

        var securityRequirement = new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        };

        if (document.Paths is not null)
        {
            foreach (var pathItem in document.Paths.Values)
            {
                if (pathItem.Operations is null)
                    continue;

                foreach (var operation in pathItem.Operations)
                {
                    if (operation.Value is null)
                        continue;
                    operation.Value.Security ??= [];
                    operation.Value.Security.Add(securityRequirement);
                }
            }
        }

        return Task.CompletedTask;
    });
});
builder.Services.AddModules(builder.Configuration);
builder.Services.AddInfrastructure(databaseConnectionString);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Authentication = new ScalarAuthenticationOptions
        {
            PreferredSecuritySchemes = ["Bearer"]
        };
    });
    app.ApplyMigrations();
}
if (args.Contains("--seed"))
{
    using IServiceScope scope = app.Services.CreateScope();
    DatabaseSeeder seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAllAsync();
    await seeder.ValidateRequiredSeedDataAsync(); 
    return;
}
app.UsePresentation();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();