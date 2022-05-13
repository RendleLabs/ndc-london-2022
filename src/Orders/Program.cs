using Ingredients.Protos;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Orders.PubSub;
using Orders.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(k =>
{
    k.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc();

var ingredientsUri = builder.Configuration.GetServiceUri("Ingredients")
                     ?? new Uri("https://localhost:5003");

builder.Services.AddGrpcClient<IngredientsService.IngredientsServiceClient>(o =>
{
    o.Address = ingredientsUri;
});

builder.Services.AddOrderPubSub();

var app = builder.Build();

app.MapGrpcService<OrdersImpl>();

app.Run();
