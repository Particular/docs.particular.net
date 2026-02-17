using Microsoft.Extensions.Logging;

namespace BuyersRemorseTimeoutRequest;

class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {

    }

    #region BuyersRemorseTimeoutRequest

    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {orderId}", message.OrderId);

        logger.LogInformation("Starting cool down period for order #{orderId}.", Data.OrderId);
        await RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
    }

    #endregion
}

internal class BuyersRemorseIsOver { }

public class BuyersRemorseData : ContainSagaData
{
    public string? OrderId { get; set; }
}

internal class PlaceOrder
{
    public string? OrderId { get; set; }
}