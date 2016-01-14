using System.Linq;
using Microsoft.AspNet.SignalR;
using NServiceBus;
using Store.Messages.Events;

public class OrderIsReadyHandler :  IHandleMessages<DownloadIsReady>
{
    public void Handle(DownloadIsReady message)
    {
        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<OrdersHub>();
            
        context.Clients.Client(message.ClientId).orderReady(new
            {
                message.OrderNumber,
                ProductUrls = message.ProductUrls.Select(pair => new { Id = pair.Key, Url = pair.Value }).ToArray()
            });
    }
}