using Grpc.Core;
using Ingredients.Protos;

namespace Ingredients.Services;

public class IngredientsImpl : IngredientsService.IngredientsServiceBase
{
    private readonly ILogger<IngredientsImpl> _logger;

    public IngredientsImpl(ILogger<IngredientsImpl> logger)
    {
        _logger = logger;
    }

    public override Task<GetToppingsResponse> GetToppings(GetToppingsRequest request, ServerCallContext context)
    {
        return base.GetToppings(request, context);
    }
}