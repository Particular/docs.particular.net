using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Receiver
{
    class SomeMessageHandler(ILogger<SomeMessageHandler> logger) : IHandleMessages<SomeMessage>
    {
        public Task Handle(SomeMessage message, IMessageHandlerContext context)
        {
           logger.LogInformation("Got message {Counter}", message.Counter);
            return Task.CompletedTask;
        }
    }

}
