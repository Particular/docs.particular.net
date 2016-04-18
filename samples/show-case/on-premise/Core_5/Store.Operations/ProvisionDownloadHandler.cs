using System.Diagnostics;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.RequestResponse;

public class ProvisionDownloadHandler : IHandleMessages<ProvisionDownloadRequest>
{
    static ILog log = LogManager.GetLogger<ProvisionDownloadHandler>();
    IBus bus;

    public ProvisionDownloadHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(ProvisionDownloadRequest message)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        log.InfoFormat("Provision the products and make the Urls available to the Content management for download ...[{0}] product(s) to provision", string.Join(", ", message.ProductIds));

        bus.Reply(new ProvisionDownloadResponse
            {
                OrderNumber = message.OrderNumber,
                ProductIds = message.ProductIds,
                ClientId = message.ClientId
            });
    }
}