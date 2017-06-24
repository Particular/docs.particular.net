using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        #region Reply

        var orderAccepted = new OrderReceived
        {
            OrderId = message.OrderId
        };
        return context.Reply(orderAccepted);

        #endregion
    }
}