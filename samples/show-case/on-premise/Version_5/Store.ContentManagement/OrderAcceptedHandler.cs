using System.Diagnostics;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Events;
using Store.Messages.RequestResponse;

public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();
    IBus bus;

    public OrderAcceptedHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(OrderAccepted message)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        log.InfoFormat("Order # {0} has been accepted, Let's provision the download -- Sending ProvisionDownloadRequest to the Store.Operations endpoint", message.OrderNumber);
            
        //send out a request (a event will be published when the response comes back)
        bus.Send<ProvisionDownloadRequest>(r =>
        {
            r.ClientId = message.ClientId;
            r.OrderNumber = message.OrderNumber;
            r.ProductIds = message.ProductIds;
        });

    }
}