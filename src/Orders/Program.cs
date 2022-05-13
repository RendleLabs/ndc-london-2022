using System.Security.Claims;
using AuthHelp;
using Ingredients.Protos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateActor = false,
            ValidateLifetime = false,
            IssuerSigningKey = JwtHelper.SecurityKey
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireClaim(ClaimTypes.Name);
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<OrdersImpl>();

app.Map("/generateJwt", context =>
    context.Response.WriteAsync(JwtHelper.GenerateJwtToken(context.Request.Query["name"])));

app.Run();
