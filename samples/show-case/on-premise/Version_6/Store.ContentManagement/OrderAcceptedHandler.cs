namespace Store.ContentManagement
{
    using System;
    using System.Diagnostics;
    using Common;
    using Messages.RequestResponse;
    using Messages.Events;
    using NServiceBus;

    public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
    {

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

            Console.WriteLine("Order # {0} has been accepted, Let's provision the download -- Sending ProvisionDownloadRequest to the Store.Operations endpoint", message.OrderNumber);
            
            //send out a request (a event will be published when the response comes back)
            bus.Send<ProvisionDownloadRequest>(r =>
            {
                r.ClientId = message.ClientId;
                r.OrderNumber = message.OrderNumber;
                r.ProductIds = message.ProductIds;
            });

        }
    }
}