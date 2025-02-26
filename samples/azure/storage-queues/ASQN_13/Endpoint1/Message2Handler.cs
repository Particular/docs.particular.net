using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class Message2Handler :
    IHandleMessages<Message2>
{
    private static readonly ILogger<Message2Handler> logger =
   LoggerFactory.Create(builder =>
   {
       builder.AddConsole();
   }).CreateLogger<Message2Handler>();

    public Task Handle(Message2 message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received Message2: {message.Property}");
        return Task.CompletedTask;
    }
}