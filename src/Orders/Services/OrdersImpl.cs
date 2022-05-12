using Grpc.Core;
using Ingredients.Protos;
using Orders.Protos;

namespace Orders.Services;

public class OrdersImpl : OrderService.OrderServiceBase
{
    private readonly IngredientsService.IngredientsServiceClient _ingredients;

    public OrdersImpl(IngredientsService.IngredientsServiceClient ingredients)
    {
        _ingredients = ingredients;
    }

    public override async Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request, ServerCallContext context)
    {
        await _ingredients.DecrementCrustsAsync(new DecrementCrustsRequest
        {
            CrustId = request.CrustId
        });
        await _ingredients.DecrementToppingsAsync(new DecrementToppingsRequest
        {
            ToppingIds = { request.ToppingIds }
        });
        
        return new PlaceOrderResponse();
    }
}