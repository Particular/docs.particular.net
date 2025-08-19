using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
#region theshipordersaga

public class ShipOrderSaga(ILogger<ShipOrderSaga> logger) :
    Saga<ShipOrderSagaData>,
    IAmStartedByMessages<ShipOrder>,
    IHandleTimeouts<CompleteOrder>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId).ToMessage<ShipOrder>(msg => msg.OrderId);
    }

    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order Shipped. OrderId {message.OrderId}");
        Data.OrderId = message.OrderId;

        logger.LogInformation("Order will complete in 5 seconds");
        CompleteOrder timeoutData = new();
        return RequestTimeout(context, TimeSpan.FromSeconds(5), timeoutData);
    }

    public Task Timeout(CompleteOrder state, IMessageHandlerContext context)
    {
        logger.LogInformation($"Saga with OrderId {Data.OrderId} about to complete");
        MarkAsComplete();

        state.OrderId = Data.OrderId;

        return ReplyToOriginator(context, state);
    }
}

#endregion