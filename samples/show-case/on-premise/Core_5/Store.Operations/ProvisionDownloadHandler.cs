using System.Diagnostics;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.RequestResponse;

public class ProvisionDownloadHandler :
    IHandleMessages<ProvisionDownloadRequest>
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

        var products = string.Join(", ", message.ProductIds);
        log.Info($"Provision the products and make the Urls available to the Content management for download ...[{products}] product(s) to provision");

        var response = new ProvisionDownloadResponse
        {
            OrderNumber = message.OrderNumber,
            ProductIds = message.ProductIds,
            ClientId = message.ClientId
        };
        bus.Reply(response);
    }
}