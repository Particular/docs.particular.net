using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class OrderSubmittedHandler(ILogger<OrderSubmittedHandler> logger) :
    IHandleMessages<OrderSubmitted>
{

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order {OrderId} worth {Value} submitted", message.OrderId, message.Value);

        #region StoreUserData

        var nhibernateSession = context.SynchronizedStorageSession.Session();
        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };
        nhibernateSession.Save(order);

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