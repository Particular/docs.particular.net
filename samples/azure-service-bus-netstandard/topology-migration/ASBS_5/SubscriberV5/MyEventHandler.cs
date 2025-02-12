using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace SubscriberV5
{
    public class MyEventHandler : IHandleMessages<MyEvent>
    {
        private readonly ILogger<MyEventHandler> logger;

        public MyEventHandler(ILogger<MyEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(MyEvent message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received MyEvent");
            return Task.CompletedTask;
        }
    }
}
