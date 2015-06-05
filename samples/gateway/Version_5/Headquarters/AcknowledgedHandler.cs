using System;
using NServiceBus;

#region AcknowledgedHandler
public class AcknowledgedHandler : IHandleMessages<PriceUpdateAcknowledged>
{
    public void Handle(PriceUpdateAcknowledged message)
    {
        Console.WriteLine("Price update received by: " + message.BranchOffice);
    }
}

#endregion