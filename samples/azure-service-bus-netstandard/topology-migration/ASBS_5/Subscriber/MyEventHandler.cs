using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace Subscriber
{
    public class MyEventHandler(ILogger<MyEventHandler> logger) : IHandleMessages<MyEvent>
    {
        public Task Handle(MyEvent message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received MyEvent");
            return Task.CompletedTask;
        }
    }
}
