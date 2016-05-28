using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Events;
using Store.Messages.RequestResponse;

public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();

    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        log.Info("Order # {message.OrderNumber} has been accepted, Let's provision the download -- Sending ProvisionDownloadRequest to the Store.Operations endpoint");

        //send out a request (a event will be published when the response comes back)
        return context.Send<ProvisionDownloadRequest>(r =>
        {
            r.ClientId = message.ClientId;
            r.OrderNumber = message.OrderNumber;
            r.ProductIds = message.ProductIds;
        });

    }

}