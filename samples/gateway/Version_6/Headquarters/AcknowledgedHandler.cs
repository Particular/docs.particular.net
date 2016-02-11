using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared;

#region AcknowledgedHandler
public class AcknowledgedHandler : IHandleMessages<PriceUpdateAcknowledged>
{
    public Task Handle(PriceUpdateAcknowledged message, IMessageHandlerContext context)
    {
        Console.WriteLine("Price update received by: " + message.BranchOffice);
        return Task.FromResult(0);
    }
}

#endregion