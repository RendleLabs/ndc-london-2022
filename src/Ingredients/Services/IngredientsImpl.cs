using Grpc.Core;
using Ingredients.Data;
using Ingredients.Protos;

namespace Ingredients.Services;

internal class IngredientsImpl : IngredientsService.IngredientsServiceBase
{
    private readonly IToppingData _toppingData;
    private readonly ICrustData _crustData;
    private readonly ILogger<IngredientsImpl> _logger;

    public IngredientsImpl(IToppingData toppingData, ICrustData crustData, ILogger<IngredientsImpl> logger)
    {
        _toppingData = toppingData;
        _crustData = crustData;
        _logger = logger;
    }

    public override async Task<GetToppingsResponse> GetToppings(GetToppingsRequest request, ServerCallContext context)
    {
        try
        {
            var toppings = await _toppingData.GetAsync(context.CancellationToken);

            _logger.LogInformation("Found {Count} toppings", toppings.Count);

            var response = new GetToppingsResponse
            {
                Toppings =
                {
                    toppings.Select(t => new Topping
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Price = t.Price
                    })
                }
            };

            return response;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new RpcException(new Status(StatusCode.Internal, ex.Message, ex));
        }
    }

    public override async Task<GetCrustsResponse> GetCrusts(GetCrustsRequest request, ServerCallContext context)
    {
        try
        {
            var crusts = await _crustData.GetAsync(context.CancellationToken);
            _logger.LogInformation("Found {Count} crusts", crusts.Count);

            var response = new GetCrustsResponse
            {
                Crusts =
                {
                    crusts.Select(c => new Crust
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Size = c.Size,
                        Price = c.Price
                    })
                }
            };

            return response;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new RpcException(new Status(StatusCode.Internal, ex.Message, ex));
        }
    }
}