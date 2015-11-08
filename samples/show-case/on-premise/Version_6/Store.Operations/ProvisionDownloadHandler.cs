using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.RequestResponse;

public class ProvisionDownloadHandler : IHandleMessages<ProvisionDownloadRequest>
{
    IBus bus;

    public ProvisionDownloadHandler(IBus bus)
    {
        this.bus = bus;
    }

    public async Task Handle(ProvisionDownloadRequest message)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        Console.WriteLine("Provision the products and make the Urls available to the Content management for download ...[{0}] product(s) to provision", string.Join(", ", message.ProductIds));

        await bus.ReplyAsync(new ProvisionDownloadResponse
            {
                OrderNumber = message.OrderNumber,
                ProductIds = message.ProductIds,
                ClientId = message.ClientId
            });
    }
}
