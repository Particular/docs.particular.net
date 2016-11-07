using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Events;
using Store.Messages.RequestResponse;

public class ProvisionDownloadResponseHandler :
    IHandleMessages<ProvisionDownloadResponse>
{
    static ILog log = LogManager.GetLogger<ProvisionDownloadResponseHandler>();

    Dictionary<string, string> productIdToUrlMap = new Dictionary<string, string>
        {
            {"videos", "http://particular.net/videos-and-presentations"},
            {"training", "http://particular.net/onsite-training"},
            {"documentation", "http://docs.particular.net/"},
            {"customers", "http://particular.net/customers"},
            {"platform", "http://particular.net/service-platform"},
        };

    public Task Handle(ProvisionDownloadResponse message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        log.Info($"Download for Order # {message.OrderNumber} has been provisioned, Publishing Download ready event");

        log.Info($"Downloads for Order #{message.OrderNumber} is ready, publishing it.");
        var downloadIsReady = new DownloadIsReady
        {
            OrderNumber = message.OrderNumber,
            ClientId = message.ClientId,
            ProductUrls = new Dictionary<string, string>()
        };

        foreach (var productId in message.ProductIds)
        {
            downloadIsReady.ProductUrls.Add(productId, productIdToUrlMap[productId]);
        }
        return context.Publish(downloadIsReady);

    }

}