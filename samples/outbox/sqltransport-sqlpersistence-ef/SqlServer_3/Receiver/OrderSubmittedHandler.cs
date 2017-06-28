using NServiceBus;
using System.Threading.Tasks;
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

        var storageContext = context.SynchronizedStorageSession.SqlPersistenceSession();

        var dbConnection = storageContext.Connection;
        using (var receiverDataContext = new ReceiverDataContext(dbConnection))
        {
            receiverDataContext.Database.UseTransaction(storageContext.Transaction);
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

}
