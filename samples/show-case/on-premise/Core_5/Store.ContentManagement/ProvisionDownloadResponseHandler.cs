using System.Collections.Generic;
using System.Diagnostics;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Events;
using Store.Messages.RequestResponse;

public class ProvisionDownloadResponseHandler : IHandleMessages<ProvisionDownloadResponse>
{
    static ILog log = LogManager.GetLogger<ProvisionDownloadResponseHandler>();
    IBus bus;

    public ProvisionDownloadResponseHandler(IBus bus)
    {
        this.bus = bus;
    }

    Dictionary<string, string> productIdToUrlMap = new Dictionary<string, string>
        {
            {"videos", "http://particular.net/videos-and-presentations"},
            {"training", "http://particular.net/onsite-training"},
            {"documentation", "http://docs.particular.net/"},
            {"customers", "http://particular.net/customers"},
            {"platform", "http://particular.net/service-platform"},
        };

    public void Handle(ProvisionDownloadResponse message)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        log.Info($"Download for Order # {message.OrderNumber} has been provisioned, Publishing Download ready event");

        bus.Publish<DownloadIsReady>(e =>
        {
            e.OrderNumber = message.OrderNumber;
            e.ClientId = message.ClientId;
            e.ProductUrls = new Dictionary<string, string>();

            foreach (var productId in message.ProductIds)
            {
                e.ProductUrls.Add(productId, productIdToUrlMap[productId]);
            }
        });

        log.Info($"Downloads for Order #{message.OrderNumber} is ready, publishing it.");
    }
}