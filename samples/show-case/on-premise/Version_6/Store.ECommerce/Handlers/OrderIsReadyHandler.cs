namespace Store.ECommerce.Handlers
{
    using System.Linq;
    using Microsoft.AspNet.SignalR;
    using Messages.Events;
    using NServiceBus;

    public class OrderIsReadyHandler :  IHandleMessages<DownloadIsReady>
    {
        public void Handle(DownloadIsReady message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<OrdersHub>();
            
            context.Clients.Client(message.ClientId).orderReady(new
                {
                    message.OrderNumber,
                    ProductUrls = message.ProductUrls.Select(pair => new { Id = pair.Key, Url = pair.Value }).ToArray()
                });
        }
    }
}