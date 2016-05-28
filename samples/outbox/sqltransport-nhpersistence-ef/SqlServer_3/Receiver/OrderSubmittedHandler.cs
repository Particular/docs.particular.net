using NServiceBus;
using System.Data.Common;
using System.Threading.Tasks;
using NHibernate;
using NServiceBus.Logging;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.InfoFormat($"Order {message.OrderId} worth {message.Value} submitted");

        #region StoreUserData

        var storageContext = context.SynchronizedStorageSession.Session();

        using (var receiverDataContext = new ReceiverDataContext(storageContext.Connection))
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
        DbTransaction dbTransaction;
        using (var helper = storageContext.Connection.CreateCommand())
        {
            storageContext.Transaction.Enlist(helper);
            dbTransaction = (DbTransaction) helper.Transaction;
        }
        return dbTransaction;
    }
}
