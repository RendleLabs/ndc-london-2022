using Grpc.HealthCheck;
using Ingredients;
using Ingredients.Data;
using Ingredients.Services;
using JaegerTracing;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
builder.AddJaegerTracing();
builder.WebHost.ConfigureKestrel(k =>
{
    k.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc();
builder.Services.AddSingleton<IToppingData, ToppingData>();
builder.Services.AddSingleton<ICrustData, CrustData>();

builder.Services.AddSingleton<HealthServiceImpl>();
builder.Services.AddHostedService<HealthCheckService>();

var app = builder.Build();

app.MapGrpcService<IngredientsImpl>();
app.MapGrpcService<HealthServiceImpl>();

app.Run();
