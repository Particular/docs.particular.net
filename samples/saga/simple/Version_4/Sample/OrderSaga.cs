using System;
using log4net;
using NServiceBus;
using NServiceBus.Saga;

#region thesaga
public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>,
    IHandleTimeouts<CancelOrder>
{
    static ILog logger = LogManager.GetLogger(typeof(OrderSaga));

    public override void ConfigureHowToFindSaga()
    {
        ConfigureMapping<StartOrder>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);
        ConfigureMapping<CompleteOrder>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);
    }

    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        logger.InfoFormat("Saga with OrderId {0} received StartOrder with OrderId {1}", Data.OrderId, message.OrderId);
        Bus.SendLocal(new CompleteOrder
                           {
                               OrderId = Data.OrderId
                           });
        RequestTimeout<CancelOrder>(TimeSpan.FromMinutes(30));
    }

    public void Handle(CompleteOrder message)
    {
        logger.InfoFormat("Saga with OrderId {0} received CompleteOrder with OrderId {1}", Data.OrderId, message.OrderId);
        MarkAsComplete();
    }

    public void Timeout(CancelOrder state)
    {
        logger.InfoFormat("Complete not received soon enough OrderId {0}", Data.OrderId);
        MarkAsComplete();
    }

}
#endregion