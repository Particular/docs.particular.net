using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Dapper;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

        #region StoreUserData

        var session = context.SynchronizedStorageSession.SqlPersistenceSession();
        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };

        session.Connection.Execute("INSERT INTO [receiver].[Orders] (OrderId, Value) VALUES (@OrderId, @Value)", order);

        #endregion

        #region Reply

        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        return context.Reply(orderAccepted);

        #endregion
    }
}