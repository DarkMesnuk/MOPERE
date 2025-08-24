#region Config

using Identity.API.Configurations;
using Messenger.API.Configurations;
using TaskBook.API.Configurations;
using Mopere.Configurations;
using Mopere.Middlewares;
using Mopere.Middlewares.Filters;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false);

var isLocal = true;

var configRoot = config.Build();

var env = configRoot.GetValue<string>("Env");

if (!isLocal)
{
    var region = configRoot.GetValue<string>("AWSRegion");
    var secretBaseName = configRoot.GetValue<string>("AWSSecretName");
    var secretName = $"{env}/{secretBaseName}";

    config.AddAmazonSecretsManager(region!, secretName);   
}

if (config.Sources.Count == 1)
{
    if (env == "dev")
        config.AddJsonFile("appsettings.Development.json", optional: false);
}

configRoot = config.Build();

#endregion

#region Builder

var builder = WebApplication.CreateBuilder(args);

builder.UseSerilog(configRoot);

builder.AddMopereCors();

var services = builder.Services;

services.AddIdentityApi(configRoot);
services.AddMessengerApi(configRoot);
services.AddTaskBookApi(configRoot);

// services.AddTransient<IIdentityAccessChecker, Mopere.Chekers.IdentityAccessChecker>(); // If microservice use

services
    .AddAuth(configRoot)
    .AddSwagger()
    .AddEndpointsApiExplorer();


services.AddControllers(options => {
    options.Filters.Add<TokenAssignmentActionFilter>();
});

#endregion

#region App

var app = builder.Build();

app.Services.RunDataSeeder(configRoot).GetAwaiter();

app.UseMopereSwagger();

app.UseRouting();
app.UseCors();

app.UseAuthentication();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

#endregion