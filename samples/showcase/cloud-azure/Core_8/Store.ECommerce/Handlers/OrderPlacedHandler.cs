using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using NServiceBus;
using Store.Messages.Events;

public class OrderPlacedHandler :
    IHandleMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        var hubContext = GlobalHost.ConnectionManager.GetHubContext<OrdersHub>();
        hubContext.Clients.Client(message.ClientId)
            .orderReceived(new
            {
                message.OrderNumber,
                message.ProductIds
            });
        return Task.CompletedTask;
    }
}