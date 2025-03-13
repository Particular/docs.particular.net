using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Store.Messages.Events;
using Store.Messages.RequestResponse;

public class ProvisionDownloadResponseHandler(ILogger<ProvisionDownloadResponseHandler> logger) :
    IHandleMessages<ProvisionDownloadResponse>
{   
    Dictionary<string, string> productIdToUrlMap = new Dictionary<string, string>
        {
            {"videos", "https://particular.net/videos-and-presentations"},
            {"training", "https://particular.net/onsite-training"},
            {"documentation", "https://docs.particular.net/"},
            {"customers", "https://particular.net/customers"},
            {"platform", "https://particular.net/service-platform"},
        };

    public Task Handle(ProvisionDownloadResponse message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        logger.LogInformation($"Download for Order # {message.OrderNumber} has been provisioned, Publishing Download ready event");

        logger.LogInformation($"Downloads for Order #{message.OrderNumber} is ready, publishing it.");
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