using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Subscriber1.Contracts;


class Consumer1EventHandler :
    IHandleMessages<Consumer1Contract>
{
    private static readonly ILogger<Consumer1EventHandler> logger =
      LoggerFactory.Create(builder =>
      {
          builder.AddConsole();
      }).CreateLogger<Consumer1EventHandler>();

    public Task Handle(Consumer1Contract message, IMessageHandlerContext context)
    {
        logger.LogInformation(message.Consumer1Property);
        return Task.CompletedTask;
    }
}