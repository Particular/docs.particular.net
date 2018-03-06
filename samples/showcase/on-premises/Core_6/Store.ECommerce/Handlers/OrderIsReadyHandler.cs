using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using NServiceBus;
using Store.Messages.Events;

public class OrderIsReadyHandler :
    IHandleMessages<DownloadIsReady>
{
    public Task Handle(DownloadIsReady message, IMessageHandlerContext context)
    {
        var hubContext = GlobalHost.ConnectionManager.GetHubContext<OrdersHub>();
        hubContext.Clients.Client(message.ClientId)
            .orderReady(new
            {
                message.OrderNumber,
                ProductUrls = message.ProductUrls.Select(pair => new { Id = pair.Key, Url = pair.Value }).ToArray()
            });
        return Task.CompletedTask;
    }
}