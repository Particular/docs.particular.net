namespace Store.ECommerce.Handlers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNet.SignalR;
    using Messages.Events;
    using NServiceBus;

    public class OrderIsReadyHandler :  IHandleMessages<DownloadIsReady>
    {
        public Task Handle(DownloadIsReady message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<OrdersHub>();
            
            context.Clients.Client(message.ClientId).orderReady(new
                {
                    message.OrderNumber,
                    ProductUrls = message.ProductUrls.Select(pair => new { Id = pair.Key, Url = pair.Value }).ToArray()
                });
            return Task.FromResult(0);
        }
    }
}