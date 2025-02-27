using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class Message1Handler :
    IHandleMessages<Message1>
{
    private readonly ILogger<Message1Handler> _logger;

    public Message1Handler(ILogger<Message1Handler> logger)
    {
        _logger = logger;
    }

    public Task Handle(Message1 message, IMessageHandlerContext context)
    {
        _logger.LogInformation($"Received Message1: {message.Property}");
        var message2 = new Message2
        {
            Property = "Hello from Endpoint2"
        };
        return context.Reply(message2);
    }
}
