using log4net;
using NServiceBus;
using Shared;

#region AcknowledgedHandler

public class AcknowledgedHandler : IHandleMessages<PriceUpdateAcknowledged>
{
    static ILog log = LogManager.GetLogger(typeof(AcknowledgedHandler));

    public void Handle(PriceUpdateAcknowledged message)
    {
        log.Info("Price update received by: " + message.BranchOffice);
    }
}

#endregion