using System;
using NServiceBus;
using Shared;

#region AcknowledgedHandler
public class AcknowledgedHandler : IHandleMessages<PriceUpdateAcknowledged>
{
    public void Handle(PriceUpdateAcknowledged message)
    {
        Console.WriteLine("Price update received by: " + message.BranchOffice);
    }
}

#endregion