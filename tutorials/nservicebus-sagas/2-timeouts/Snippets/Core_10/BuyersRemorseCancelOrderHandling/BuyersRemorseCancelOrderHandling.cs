using Microsoft.Extensions.Logging;

namespace BuyersRemorseCancelOrderHandling;

#region BuyersRemorseCancelOrderHandling

class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>,
    IAmStartedByMessages<PlaceOrder>,
    IHandleMessages<CancelOrder>,
    IHandleTimeouts<BuyersRemorseIsOver>
{
  protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<PlaceOrder>(message => message.OrderId)
            .ToMessage<CancelOrder>(message => message.OrderId);
    }

    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);

        Data.OrderId = message.OrderId;

        return Task.CompletedTask;
    }

    public async Task Timeout(BuyersRemorseIsOver state, IMessageHandlerContext context)
    {
        logger.LogInformation("Cooling down period for order #{OrderId} has elapsed.", Data.OrderId);

        var orderPlaced = new OrderPlaced
        {
            OrderId = Data.OrderId
        };

        await context.Publish(orderPlaced);

        MarkAsComplete();
    }

    public Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order #{OrderId} was cancelled.", message.OrderId);

        //TODO: Possibly publish an OrderCancelled event?

        MarkAsComplete();

        return Task.CompletedTask;
    }
}

#endregion

internal class BuyersRemorseIsOver
{
}

internal class PlaceOrder
{
    public object? OrderId { get; set; }
}

internal class OrderPlaced
{
    public object? OrderId { get; set; }
}

internal class BuyersRemorseData : ContainSagaData
{
    public object? OrderId { get; set; }
}

internal class CancelOrder
{
    public object? OrderId { get; set; }
}