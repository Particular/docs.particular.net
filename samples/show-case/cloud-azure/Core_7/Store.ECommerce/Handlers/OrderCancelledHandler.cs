using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using NServiceBus;
using Store.Messages.Events;

public class OrderCancelledHandler :
    IHandleMessages<OrderCancelled>
{
    public Task Handle(OrderCancelled message, IMessageHandlerContext context)
    {
        var hubContext = GlobalHost.ConnectionManager.GetHubContext<OrdersHub>();

        hubContext.Clients.Client(message.ClientId)
            .orderCancelled(new
            {
                message.OrderNumber,
            });
        return Task.CompletedTask;
    }
}
