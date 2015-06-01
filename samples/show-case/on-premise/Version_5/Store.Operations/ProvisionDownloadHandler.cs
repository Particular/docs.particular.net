namespace Store.Operations
{
    using System;
    using System.Diagnostics;
    using Common;
    using Messages.RequestResponse;
    using NServiceBus;

    public class ProvisionDownloadHandler : IHandleMessages<ProvisionDownloadRequest>
    {
        public IBus Bus { get; set; }

        public void Handle(ProvisionDownloadRequest message)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            Console.WriteLine("Provision the products and make the Urls available to the Content management for download ...[{0}] product(s) to provision", string.Join(", ", message.ProductIds));

            Bus.Reply(new ProvisionDownloadResponse
                {
                    OrderNumber = message.OrderNumber,
                    ProductIds = message.ProductIds,
                    ClientId = message.ClientId
                });
        }
    }
}