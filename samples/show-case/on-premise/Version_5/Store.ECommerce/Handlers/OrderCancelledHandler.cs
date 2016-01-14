using Microsoft.AspNet.SignalR;
using NServiceBus;
using Store.Messages.Events;

public class OrderCancelledHandler : IHandleMessages<OrderCancelled>
{
    public void Handle(OrderCancelled message)
    {
        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<OrdersHub>();

        context.Clients.Client(message.ClientId).orderCancelled(new
        {
            message.OrderNumber,
        });
    }
}