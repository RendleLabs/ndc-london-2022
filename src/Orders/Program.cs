using Ingredients.Protos;
using Orders.PubSub;
using Orders.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var ingredientsUri = builder.Configuration.GetServiceUri("Ingredients", "https")
                     ?? new Uri("https://localhost:5003");

builder.Services.AddGrpcClient<IngredientsService.IngredientsServiceClient>(o =>
{
    o.Address = ingredientsUri;
});

builder.Services.AddOrderPubSub();

var app = builder.Build();

app.MapGrpcService<OrdersImpl>();

app.Run();
