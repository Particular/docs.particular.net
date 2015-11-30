using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Events;
using Store.Messages.RequestResponse;

public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{

    public async Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        Console.WriteLine("Order # {0} has been accepted, Let's provision the download -- Sending ProvisionDownloadRequest to the Store.Operations endpoint", message.OrderNumber);
            
        //send out a request (a event will be published when the response comes back)
        await context.Send<ProvisionDownloadRequest>(r =>
        {
            r.ClientId = message.ClientId;
            r.OrderNumber = message.OrderNumber;
            r.ProductIds = message.ProductIds;
        });

    }
    
}