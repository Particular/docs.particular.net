using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class Message1Handler(ILogger<Message1Handler> logger) :
    IHandleMessages<Message1>
{   
    public Task Handle(Message1 message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received Message1: {message.Property}");
        var message2 = new Message2
        {
            Property = "Hello from Endpoint2"
        };
        return context.Reply(message2);
    }
}
