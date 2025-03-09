using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class Message2Handler :
    IHandleMessages<Message2>
{
    private readonly ILogger<Message2Handler> _logger;
    public Message2Handler(ILogger<Message2Handler> logger)
    {
        _logger = logger;
    }

    public Task Handle(Message2 message, IMessageHandlerContext context)
    {
        _logger.LogInformation($"Received Message2: {message.Property}");
        return Task.CompletedTask;
    }
}
