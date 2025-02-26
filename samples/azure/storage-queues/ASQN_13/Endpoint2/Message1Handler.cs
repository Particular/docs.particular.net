using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class Message1Handler :
    IHandleMessages<Message1>
{
    private static readonly ILogger<Message1Handler> logger =
     LoggerFactory.Create(builder =>
     {
         builder.AddConsole();
     }).CreateLogger<Message1Handler>();

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