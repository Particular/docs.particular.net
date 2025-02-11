using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace SubscriberV4
{
    public class MyOtherEventHandler : IHandleMessages<MyOtherEvent>
    {
        private readonly ILogger<MyOtherEventHandler> logger;

        public MyOtherEventHandler(ILogger<MyOtherEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(MyOtherEvent message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received MyOtherEvent");
            return Task.CompletedTask;
        }
    }
}
