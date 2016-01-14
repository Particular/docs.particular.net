using System;
using System.Collections.Generic;
using System.Diagnostics;
using NServiceBus;
using Store.Messages.Events;
using Store.Messages.RequestResponse;

public class ProvisionDownloadResponseHandler : IHandleMessages<ProvisionDownloadResponse>
{
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

        Console.WriteLine("Download for Order # {0} has been provisioned, Publishing Download ready event", message.OrderNumber);
         
        bus.Publish<DownloadIsReady>(e =>
        {
            e.OrderNumber = message.OrderNumber;
            e.ClientId = message.ClientId;
            e.ProductUrls = new Dictionary<string, string>();

            foreach (string productId in message.ProductIds)
            {
                e.ProductUrls.Add(productId, productIdToUrlMap[productId]);
            }
        });

        Console.WriteLine("Downloads for Order #{0} is ready, publishing it.", message.OrderNumber);
    }
}