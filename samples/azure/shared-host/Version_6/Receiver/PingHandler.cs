using System.Threading.Tasks;
using NServiceBus;

#region AzureMultiHost_PingHandler

public class PingHandler : IHandleMessages<Ping>
{
    public async Task Handle(Ping message, IMessageHandlerContext context)
    {
        VerificationLogger.Write("Receiver", "Got Ping and will reply with Pong");
        await context.Reply(new Pong());
    }
}

#endregion
