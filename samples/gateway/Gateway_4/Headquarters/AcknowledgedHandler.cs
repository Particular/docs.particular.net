using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

#region AcknowledgedHandler
public class AcknowledgedHandler :
    IHandleMessages<PriceUpdateAcknowledged>
{
    static ILog log = LogManager.GetLogger<AcknowledgedHandler>();

    public Task Handle(PriceUpdateAcknowledged message, IMessageHandlerContext context)
    {
        log.Info($"Price update received by: {message.BranchOffice}");
        return Task.CompletedTask;
    }
}

#endregion