using System.Linq;
using Microsoft.AspNet.SignalR;
using NServiceBus;
using Store.Messages.Events;

public class OrderIsReadyHandler :
    IHandleMessages<DownloadIsReady>
{
    public void Handle(DownloadIsReady message)
    {
        var hubContext = GlobalHost.ConnectionManager.GetHubContext<OrdersHub>();

        hubContext.Clients.Client(message.ClientId)
            .orderReady(new
            {
                message.OrderNumber,
                ProductUrls = message.ProductUrls.Select(pair => new { Id = pair.Key, Url = pair.Value }).ToArray()
            });
    }
}