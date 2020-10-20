using System.Threading.Tasks;
using NServiceBus;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId
        };
        return context.Reply(orderAccepted);
    }
}