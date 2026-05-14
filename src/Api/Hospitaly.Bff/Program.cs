using Hospitaly.Bff.Controllers;
using Hospitaly.Bff.Extensions;
using Hospitaly.Bff.Services;   
using Hospitaly.Common.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new GlobalRoutePrefixConvention("bff"));
});
builder.AddAuthenticationInternal();
builder.AddReverseProxy();
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<UserDataService>(options =>
{
    var apiUrl = builder.Configuration.GetSection("ApiUrls:Main").Value;
    if (string.IsNullOrEmpty(apiUrl))
    {
        throw new Exception("ApiUrl is missing in the configuration file");
    }
    options.BaseAddress = new Uri(apiUrl);
});

builder.Services.AddHttpClient<UserRegistrationService>(options =>
{
    var apiUrl = builder.Configuration.GetSection("ApiUrls:Main").Value;
    if (string.IsNullOrEmpty(apiUrl))
    {
        throw new Exception("ApiUrl is missing in the configuration file");
    }
    options.BaseAddress = new Uri(apiUrl);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UsePresentation();

app.UseHttpsRedirection();

app.UseCors(BffController.CorsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();
app.MapControllers();

app.Run();
