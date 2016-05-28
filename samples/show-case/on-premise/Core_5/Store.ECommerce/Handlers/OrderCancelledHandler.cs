using Microsoft.AspNet.SignalR;
using NServiceBus;
using Store.Messages.Events;

public class OrderCancelledHandler : IHandleMessages<OrderCancelled>
{
    public void Handle(OrderCancelled message)
    {
        var hubContext = GlobalHost.ConnectionManager.GetHubContext<OrdersHub>();

        hubContext.Clients.Client(message.ClientId)
            .orderCancelled(new
        {
            message.OrderNumber,
        });
    }
}