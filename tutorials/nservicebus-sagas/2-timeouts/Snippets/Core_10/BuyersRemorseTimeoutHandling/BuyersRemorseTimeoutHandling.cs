using Microsoft.Extensions.Logging;

namespace BuyersRemorseTimeoutHandling;

#region BuyersRemorseTimeoutHandling

class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>,
    IAmStartedByMessages<PlaceOrder>,
    IHandleTimeouts<BuyersRemorseIsOver>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<PlaceOrder>(message => message.OrderId)
            .ToMessage<CancelOrder>(message => message.OrderId);
    }

    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);
        Data.OrderId = message.OrderId;

        logger.LogInformation("Starting cool down period for order #{OrderId}.", Data.OrderId);
        await RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
    }

    public async Task Timeout(BuyersRemorseIsOver timeout, IMessageHandlerContext context)
    {
        logger.LogInformation("Cooling down period for order #{OrderId} has elapsed.", Data.OrderId);

        var orderPlaced = new OrderPlaced
        {
            OrderId = Data.OrderId
        };

        await context.Publish(orderPlaced);

        MarkAsComplete();
    }


}

#endregion

internal class OrderPlaced
{
    public string? OrderId { get; set; }
}

internal class BuyersRemorseIsOver
{
}

internal class PlaceOrder
{
    public string? OrderId { get; set; }
}

internal class BuyersRemorseData : ContainSagaData
{
    public string? OrderId { get; set; }
}