using System;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

#region thesaga

public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleTimeouts<CompleteOrder>
{
    IBus bus;
    static ILog logger = LogManager.GetLogger<OrderSaga>();

    public OrderSaga(IBus bus)
    {
        this.bus = bus;
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);
    }

    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        string orderDescription = "The saga for order " + message.OrderId;
        Data.OrderDescription = orderDescription;
        logger.InfoFormat("Received StartOrder message {0}. Starting Saga", Data.OrderId);
        logger.Info("Order will complete in 5 seconds");
        CompleteOrder timeoutData = new CompleteOrder
        {
            OrderDescription = orderDescription
        };
        RequestTimeout(TimeSpan.FromSeconds(5), timeoutData);
    }

    public void Timeout(CompleteOrder state)
    {
        logger.InfoFormat("Saga with OrderId {0} completed", Data.OrderId);
        bus.Publish(new OrderCompleted
        {
            OrderId = Data.OrderId
        });
        MarkAsComplete();
    }

}

#endregion