using Couchbase.Extensions.DependencyInjection;
using Gems.Infrastructure.Providers;
using Grpc.Gems.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

//read in environment varibles to get database configuration
builder.WebHost.ConfigureAppConfiguration(
    (hostingContext, config) => {
        config.AddEnvironmentVariables(prefix: "cbsettings_");
});

/* ** 
    required in order to setup MacOS support 
    for Kestrel
    https://docs.microsoft.com/en-us/aspnet/core/grpc/troubleshoot?view=aspnetcore-6.0#unable-to-start-aspnet-core-grpc-app-on-macos

    check environment and only enable running without 
    tls in development environment
** */
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenLocalhost(
            6000,
            o => o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2);
    });
}

// Add services to the container.
builder.Services.AddGrpc();

/* **
    setup Couchbase config service, bucket providers, and repository
** */
var cbConfigService = new CouchbaseConfigService(builder.Configuration);
cbConfigService.InitConfig();

builder.Services.AddSingleton<CouchbaseConfigService>(cbConfigService);
builder.Services.AddCouchbase(options =>
{
    options.ConnectionString = cbConfigService.Config.ConnectionString;
    options.UserName = cbConfigService.Config.Username;
    options.Password = cbConfigService.Config.Password;
});

builder.Services.AddCouchbaseBucket<IGemsBucketProvider>("gems");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();

/* ** 
   throw information if someone just tries to browse 
   to this endpoint to let them know you can't directly
   connect to this service
** */
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
