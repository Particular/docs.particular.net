using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Subscriber2.Contracts;

class Consumer2EventHandler :
    IHandleMessages<Consumer2Contract>
{
    private static readonly ILogger<Consumer2EventHandler> logger =
      LoggerFactory.Create(builder =>
      {
          builder.AddConsole();
      }).CreateLogger<Consumer2EventHandler>();


    public Task Handle(Consumer2Contract message, IMessageHandlerContext context)
    {
        logger.LogInformation(message.Consumer2Property);
        return Task.CompletedTask;
    }
}