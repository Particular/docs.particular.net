using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Store.Messages.Events;

public class OrderPlacedHandler :
    IHandleMessages<OrderPlaced>
{
    private IHubContext<OrdersHub> ordersHubContext;

    public OrderPlacedHandler(IHubContext<OrdersHub> ordersHubContext)
    {
        this.ordersHubContext = ordersHubContext;
    }

    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        return ordersHubContext.Clients.Client(message.ClientId).SendAsync("orderReceived",
            new
            {
                message.OrderNumber,
                message.ProductIds
            });
    }
}