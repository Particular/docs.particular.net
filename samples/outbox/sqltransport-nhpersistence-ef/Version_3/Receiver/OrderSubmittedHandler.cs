using System;
using System.Data;
using NServiceBus;
using System.Data.Common;
using System.Threading.Tasks;
using NHibernate;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        Console.WriteLine("Order {0} worth {1} submitted", message.OrderId, message.Value);

        #region StoreUserData

        ISession storageContext = context.SynchronizedStorageSession.Session();

        using (ReceiverDataContext ctx = new ReceiverDataContext(storageContext.Connection))
        {
            DbTransaction tx = ExtractTransactionFromSession(storageContext);

            ctx.Database.UseTransaction(tx);
            ctx.Orders.Add(new Order
                            {
                                OrderId = message.OrderId,
                                Value = message.Value
                            });
            ctx.SaveChanges();
        }

        #endregion

        #region Reply

        await context.Reply(new OrderAccepted
                    {
                        OrderId = message.OrderId,
                    });

        #endregion
    }

    static DbTransaction ExtractTransactionFromSession(ISession storageContext)
    {
        DbTransaction tx;
        using (IDbCommand helper = storageContext.Connection.CreateCommand())
        {
            storageContext.Transaction.Enlist(helper);
            tx = (DbTransaction) helper.Transaction;
        }
        return tx;
    }
}
