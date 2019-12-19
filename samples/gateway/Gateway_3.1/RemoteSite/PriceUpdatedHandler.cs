using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

#region PriceUpdatedHandler
public class PriceUpdatedHandler :
    IHandleMessages<PriceUpdated>
{
    static ILog log = LogManager.GetLogger<PriceUpdatedHandler>();

    public Task Handle(PriceUpdated message, IMessageHandlerContext context)
    {
        var header = context.MessageHeaders[Headers.OriginatingSite];
        log.Info($"Price update for: {message.ProductId} received. Reply over channel: {header}");

        var updateAcknowledged = new PriceUpdateAcknowledged
        {
            BranchOffice = "RemoteSite"
        };
        return context.Reply(updateAcknowledged);
    }
}

#endregion