using System.Threading.Tasks;
using NServiceBus;

#region AzureMultiHost_PongHandler

public class PongHandler :
    IHandleMessages<Pong>
{
    public Task Handle(Pong message, IMessageHandlerContext context)
    {
        VerificationLogger.Write("Sender", "Got Pong from Receiver");
        return Task.CompletedTask;
    }
}

#endregion
