using Ingredients.Data;
using Ingredients.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton<IToppingData, ToppingData>();

var app = builder.Build();

app.MapGrpcService<IngredientsImpl>();

app.Run();
