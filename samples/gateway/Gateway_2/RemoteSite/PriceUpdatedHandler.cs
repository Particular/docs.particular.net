using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

#region PriceUpdatedHandler
public class PriceUpdatedHandler : IHandleMessages<PriceUpdated>
{
    static ILog log = LogManager.GetLogger<PriceUpdatedHandler>();

    public async Task Handle(PriceUpdated message, IMessageHandlerContext context)
    {
        string messageHeader = context.MessageHeaders[Headers.OriginatingSite];
        log.InfoFormat("Price update for product: {0} received. Going to reply over channel: {1}", message.ProductId, messageHeader);

        await context.Reply(new PriceUpdateAcknowledged
                  {
                      BranchOffice = "RemoteSite"
                  });
    }
}

#endregion