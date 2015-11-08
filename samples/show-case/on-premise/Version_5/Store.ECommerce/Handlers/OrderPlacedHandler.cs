using Microsoft.AspNet.SignalR;
using NServiceBus;
using Store.Messages.Events;

public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
{
    public void Handle(OrderPlaced message)
    {
        var context = GlobalHost.ConnectionManager.GetHubContext<OrdersHub>();

        context.Clients.Client(message.ClientId).orderReceived(new
            {
                message.OrderNumber,
                message.ProductIds
            });
    }
}
