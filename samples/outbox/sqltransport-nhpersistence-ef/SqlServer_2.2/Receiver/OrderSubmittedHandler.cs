using NServiceBus;
using System.Data.Common;
using System.Data.SqlClient;
using NServiceBus.Logging;
using NServiceBus.Persistence.NHibernate;
using Microsoft.EntityFrameworkCore;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
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
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

        #region StoreUserData

        var dbConnection = storageContext.Connection as SqlConnection;
        using (var receiverDataContext = new ReceiverDataContext(dbConnection))
        {
            var dbTransaction = (DbTransaction) storageContext.DatabaseTransaction;
            receiverDataContext.Database.UseTransaction(dbTransaction);
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
