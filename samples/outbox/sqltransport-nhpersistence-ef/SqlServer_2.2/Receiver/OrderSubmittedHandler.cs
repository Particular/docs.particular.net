using NServiceBus;
using System.Data.Common;
using NServiceBus.Logging;
using NServiceBus.Persistence.NHibernate;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();
    IBus bus;
    NHibernateStorageContext storageContext;

    public OrderSubmittedHandler(IBus bus ,NHibernateStorageContext storageContext)
    {
        this.bus = bus;
        this.storageContext = storageContext;
    }

    public void Handle(OrderSubmitted message)
    {
        log.InfoFormat("Order {0} worth {1} submitted", message.OrderId, message.Value);

        #region StoreUserData

        using (var receiverDataContext = new ReceiverDataContext(storageContext.Connection))
        {
            receiverDataContext.Database.UseTransaction((DbTransaction) storageContext.DatabaseTransaction);
            var order = new Order
            {
                OrderId = message.OrderId,
                Value = message.Value
            };
            receiverDataContext.Orders.Add(order);
            receiverDataContext.SaveChanges();
        }

        #endregion

        #region Reply

        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        bus.Reply(orderAccepted);

        #endregion
    }
}
