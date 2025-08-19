using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus;
using NServiceBus.Pipeline;

public class Message1Handler:
    IHandleMessages<Message1>
{
    static ILog logger = LogManager.GetLogger<Message1Handler>();

    public Task Handle(Message1 message, IMessageHandlerContext context)
    {
        logger.Info($"Received Message1: {message.Property}");

        var message2 = new Message2
        {
            Property = "Hello from Endpoint2"
        };
        return context.Reply(message2);
    }
}