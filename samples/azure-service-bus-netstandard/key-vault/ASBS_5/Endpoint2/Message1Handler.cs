using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class Message1Handler (ILogger<Message1Handler> logger):
    IHandleMessages<Message1>
{
    public Task Handle(Message1 message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received Message1: {Property}", message.Property);

        var message2 = new Message2
        {
            Property = "Hello from Endpoint2"
        };
        return context.Reply(message2);
    }
}