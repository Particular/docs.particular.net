using System;
using NServiceBus.Logging;
using NServiceBus.Saga;

#region OrderSaga

public class OrderSaga :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleTimeouts<CompleteOrder>
{
    static ILog log = LogManager.GetLogger(typeof(OrderSaga));

    public override void ConfigureHowToFindSaga()
    {
        ConfigureMapping<StartOrder>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);
    }

    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        var orderDescription = $"The saga for order {message.OrderId}";
        Data.OrderDescription = orderDescription;
        log.Info($"Received StartOrder message {Data.OrderId}. Starting Saga");

        var shipOrder = new ShipOrder
        {
            OrderId = message.OrderId
        };

        Bus.SendLocal(shipOrder);

        log.Info("Order will complete in 5 seconds");
        var timeoutData = new CompleteOrder
        {
            OrderDescription = orderDescription
        };

        RequestTimeout(TimeSpan.FromSeconds(5), timeoutData);
    }

    public void Timeout(CompleteOrder state)
    {
        log.Info($"Saga with OrderId {Data.OrderId} completed");
        var orderCompleted = new OrderCompleted
        {
            OrderId = Data.OrderId
        };

        Bus.Publish(orderCompleted);
        MarkAsComplete();
    }
}
#endregion