using Messages;
using Microsoft.Extensions.Logging;

namespace Sales;

class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseState>, IAmStartedByMessages<PlaceOrder>, IHandleMessages<CancelOrder>, IHandleTimeouts<BuyersRemorseIsOver>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<PlaceOrder>(message => message.OrderId)
            .ToMessage<CancelOrder>(message => message.OrderId);
    }

    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {orderId}", message.OrderId);

        logger.LogInformation("Starting cool down period for order #{orderId}.", Data.OrderId);

        await RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
    }

    public async Task Timeout(BuyersRemorseIsOver state, IMessageHandlerContext context)
    {
        logger.LogInformation("Cooling down period for order #{orderId} has elapsed.", Data.OrderId);

        var orderPlaced = new OrderPlaced
        {
            OrderId = Data.OrderId
        };

        await context.Publish(orderPlaced);

        MarkAsComplete();
    }

    public Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order #{orderId} was cancelled.", message.OrderId);

        //TODO: Possibly publish an OrderCancelled event?

        MarkAsComplete();

        return Task.CompletedTask;
    }
}

internal class BuyersRemorseIsOver
{

}

public class BuyersRemorseState : ContainSagaData
{
    public string? OrderId { get; set; }
}
