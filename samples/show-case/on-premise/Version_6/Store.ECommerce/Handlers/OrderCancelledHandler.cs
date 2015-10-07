namespace Store.ECommerce.Handlers
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.SignalR;
    using Messages.Events;
    using NServiceBus;

    public class OrderCancelledHandler : IHandleMessages<OrderCancelled>
    {
        public Task Handle(OrderCancelled message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<OrdersHub>();

            context.Clients.Client(message.ClientId).orderCancelled(new
                {
                    message.OrderNumber,
                });
            return Task.FromResult(0);
        }
    }
}