using System.Collections.Generic;
using System.Threading;
using GrpcTestHelper;
using Ingredients.Data;
using Ingredients.Protos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;

namespace Ingredients.Tests;

public class IngredientsApplicationFactory : WebApplicationFactory<TestMarker>
{
    public IngredientsService.IngredientsServiceClient CreateGrpcClient()
    {
        var channel = this.CreateGrpcChannel();
        return new IngredientsService.IngredientsServiceClient(channel);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            SubToppingData(services);
            SubCrustData(services);
        });
        base.ConfigureWebHost(builder);
    }

    private static void SubToppingData(IServiceCollection services)
    {
        services.RemoveAll<IToppingData>();

        var toppings = new List<ToppingEntity>
        {
            new("cheese", "Cheese", 0.5d, 10),
            new("tomato", "Tomato", 0.75d, 10)
        };

        var toppingSub = Substitute.For<IToppingData>();

        toppingSub.GetAsync(Arg.Any<CancellationToken>())
            .Returns(toppings);

        services.AddSingleton(toppingSub);
    }
    
    private static void SubCrustData(IServiceCollection services)
    {
        services.RemoveAll<ICrustData>();

        var toppings = new List<CrustEntity>
        {
            new("thin9", "Thin 9in", 9, 5d, 10),
            new("deep9", "Deep 9in", 9, 6d, 10),
        };

        var toppingSub = Substitute.For<ICrustData>();

        toppingSub.GetAsync(Arg.Any<CancellationToken>())
            .Returns(toppings);

        services.AddSingleton(toppingSub);
    }
}