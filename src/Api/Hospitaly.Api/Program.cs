using Hospitaly.Api.Extensions;
using Hospitaly.Common.Infrastructure;
using Hospitaly.Common.Presentation;
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

builder.Services.AddOpenApi();
builder.Services.AddModules(builder.Configuration);
builder.Services.AddInfrastructure(databaseConnectionString);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.ApplyMigrations();
}

app.UsePresentation();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();