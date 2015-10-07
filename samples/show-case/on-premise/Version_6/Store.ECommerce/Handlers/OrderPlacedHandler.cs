namespace Store.ECommerce.Handlers
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.SignalR;
    using Messages.Events;
    using NServiceBus;

    public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        public Task Handle(OrderPlaced message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<OrdersHub>();

            context.Clients.Client(message.ClientId).orderReceived(new
                {
                    message.OrderNumber,
                    message.ProductIds
                });
            return Task.FromResult(0);
        }
    }
}