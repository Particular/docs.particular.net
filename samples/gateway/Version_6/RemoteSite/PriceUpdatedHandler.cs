using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared;

#region PriceUpdatedHandler
public class PriceUpdatedHandler : IHandleMessages<PriceUpdated>
{
    public async Task Handle(PriceUpdated message, IMessageHandlerContext context)
    {
        string messageHeader = context.MessageHeaders[Headers.OriginatingSite];
        Console.WriteLine("Price update for product: {0} received. Going to reply over channel: {1}", message.ProductId, messageHeader);

        await context.Reply(new PriceUpdateAcknowledged
                  {
                      BranchOffice = "RemoteSite"
                  });
    }
}

#endregion