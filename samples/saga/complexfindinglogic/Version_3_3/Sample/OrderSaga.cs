using log4net;
using NServiceBus;
using NServiceBus.Saga;

#region thesaga
public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>
{
    static ILog logger = LogManager.GetLogger(typeof(OrderSaga));

    public override void ConfigureHowToFindSaga()
    {
        ConfigureMapping<StartOrder>(s => s.OrderId, m => m.OrderId);
        ConfigureMapping<CompleteOrder>(s => s.OrderId, m => m.OrderId);
    }
#endregion

    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        logger.Info(string.Format("Saga with OrderId {0} received StartOrder with OrderId {1}", Data.OrderId, message.OrderId));
        Bus.SendLocal(new CompleteOrder
                           {
                               OrderId = Data.OrderId
                           });
    }

    public void Handle(CompleteOrder message)
    {
        logger.Info(string.Format("Saga with OrderId {0} received CompleteOrder with OrderId {1}", Data.OrderId, message.OrderId));
        MarkAsComplete();
    }

}