using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.InfoFormat($"Order {message.OrderId} worth {message.Value} submitted");

        #region StoreUserData

        var nhibernateSession = context.SynchronizedStorageSession.Session();
        nhibernateSession.Save(new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        });

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