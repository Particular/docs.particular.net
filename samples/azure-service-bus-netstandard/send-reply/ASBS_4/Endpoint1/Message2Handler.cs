using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Message2Handler :
    IHandleMessages<Message2>
{
    static readonly ILog Log = LogManager.GetLogger<Message2Handler>();

    public Task Handle(Message2 message, IMessageHandlerContext context)
    {
        Log.Info($"Received Message2: {message.Property}");
        return Task.CompletedTask;
    }
}