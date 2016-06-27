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

        log.Info($"Order # {message.OrderNumber} has been accepted, Let's provision the download -- Sending ProvisionDownloadRequest to the Store.Operations endpoint");

        //send out a request (a event will be published when the response comes back)
        var request = new ProvisionDownloadRequest
        {
            ClientId = message.ClientId,
            OrderNumber = message.OrderNumber,
            ProductIds = message.ProductIds
        };
        bus.Send(request);
    }
}