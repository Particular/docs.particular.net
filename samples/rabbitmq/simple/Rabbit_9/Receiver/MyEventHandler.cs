namespace Receiver
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NServiceBus;

    public class MyEventHandler(ILogger<MyEventHandler> logger) : IHandleMessages<MyEvent>
    {
      
        public Task Handle(MyEvent eventMessage, IMessageHandlerContext context)
        {
            logger.LogInformation($"Hello from {nameof(MyEventHandler)}");
            return Task.CompletedTask;
        }
    }
}