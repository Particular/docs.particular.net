using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Subscriber1.Contracts;


class Consumer1EventHandler :
    IHandleMessages<Consumer1Contract>
{
    private readonly ILogger<Consumer1EventHandler> logger;

    public Consumer1EventHandler()
    {
        ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        this.logger = factory.CreateLogger<Consumer1EventHandler>();
    
    }

    public Task Handle(Consumer1Contract message, IMessageHandlerContext context)
    {
        logger.LogInformation(message.Consumer1Property);
        return Task.CompletedTask;
    }
}