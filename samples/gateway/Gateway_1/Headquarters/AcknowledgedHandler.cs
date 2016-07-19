using NServiceBus;
using NServiceBus.Logging;
using Shared;

#region AcknowledgedHandler
public class AcknowledgedHandler :
    IHandleMessages<PriceUpdateAcknowledged>
{
    static ILog log = LogManager.GetLogger<AcknowledgedHandler>();

    public void Handle(PriceUpdateAcknowledged message)
    {
        log.Info($"Price update received by: {message.BranchOffice}");
    }
}

#endregion