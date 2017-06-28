using NServiceBus;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NHibernate;
using NServiceBus.Logging;
using Microsoft.EntityFrameworkCore;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

        #region StoreUserData

        var storageContext = context.SynchronizedStorageSession.Session();

        var dbConnection = (SqlConnection) storageContext.Connection;
        using (var receiverDataContext = new ReceiverDataContext(dbConnection))
        {
            var dbTransaction = ExtractTransactionFromSession(storageContext);

            receiverDataContext.Database.UseTransaction(dbTransaction);
            var order = new Order
            {
                OrderId = message.OrderId,
                Value = message.Value
            };
            receiverDataContext.Orders.Add(order);
            await receiverDataContext.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        #endregion

        #region Reply

        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        await context.Reply(orderAccepted)
            .ConfigureAwait(false);

        #endregion
    }

    static DbTransaction ExtractTransactionFromSession(ISession storageContext)
    {
        using (var helper = storageContext.Connection.CreateCommand())
        {
            storageContext.Transaction.Enlist(helper);
            return (DbTransaction) helper.Transaction;
        }
    }
}
