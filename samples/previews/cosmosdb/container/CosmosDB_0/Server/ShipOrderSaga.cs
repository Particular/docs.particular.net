using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region theshipordersaga

public class ShipOrderSaga :
    Saga<ShipOrderSagaData>,
    IAmStartedByMessages<ShipOrder>,
    IHandleTimeouts<CompleteOrder>
{
    static ILog log = LogManager.GetLogger<ShipOrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId).ToMessage<ShipOrder>(msg => msg.OrderId);
    }

    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        log.Info($"Order Shipped. OrderId {message.OrderId}");
        Data.OrderId = message.OrderId;

        log.Info("Order will complete in 5 seconds");
        var timeoutData = new CompleteOrder();
        return RequestTimeout(context, TimeSpan.FromSeconds(5), timeoutData);
    }

    public Task Timeout(CompleteOrder state, IMessageHandlerContext context)
    {
        log.Info($"Saga with OrderId {Data.OrderId} about to complete");
        MarkAsComplete();

        state.OrderId = Data.OrderId;

        return ReplyToOriginator(context, state);
    }
}

#endregion