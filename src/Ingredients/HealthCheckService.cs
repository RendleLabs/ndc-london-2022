using Grpc.Health.V1;
using Grpc.HealthCheck;
using Ingredients.Data;

namespace Ingredients;

public class HealthCheckService : BackgroundService
{
    private readonly HealthServiceImpl _impl;
    private readonly IToppingData _toppingData;

    public HealthCheckService(HealthServiceImpl impl, IToppingData toppingData)
    {
        _impl = impl;
        _toppingData = toppingData;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var toppings = await _toppingData.GetAsync(stoppingToken);
                _impl.SetStatus("ingredients", HealthCheckResponse.Types.ServingStatus.Serving);
            }
            catch (Exception e)
            {
                _impl.SetStatus("ingredients", HealthCheckResponse.Types.ServingStatus.NotServing);
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}