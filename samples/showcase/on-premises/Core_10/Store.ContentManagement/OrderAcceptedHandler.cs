using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Store.Messages.Events;
using Store.Messages.RequestResponse;

public class OrderAcceptedHandler(ILogger<OrderAcceptedHandler> logger) :
    IHandleMessages<OrderAccepted>
{

    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        logger.LogInformation("Order # {OrderNumber} has been accepted, Let's provision the download -- Sending ProvisionDownloadRequest to the Store.Operations endpoint", message.OrderNumber);

        // send out a request (a event will be published when the response comes back)
        var request = new ProvisionDownloadRequest
        {
            ClientId = message.ClientId,
            OrderNumber = message.OrderNumber,
            ProductIds = message.ProductIds
        };
        return context.Send(request);
    }
}