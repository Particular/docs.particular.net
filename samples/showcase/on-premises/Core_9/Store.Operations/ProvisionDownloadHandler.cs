using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Store.Messages.RequestResponse;

public class ProvisionDownloadHandler(ILogger<ProvisionDownloadHandler> logger) :
    IHandleMessages<ProvisionDownloadRequest>
{
    public Task Handle(ProvisionDownloadRequest message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        var products = string.Join(", ", message.ProductIds);
        logger.LogInformation("Provision the products and make the Urls available to the Content management for download ...[{Products}] product(s) to provision", products);

        var response = new ProvisionDownloadResponse
        {
            OrderNumber = message.OrderNumber,
            ProductIds = message.ProductIds,
            ClientId = message.ClientId
        };
        return context.Reply(response);
    }
}
