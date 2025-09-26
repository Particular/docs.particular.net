using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Subscriber2.Contracts;

class Consumer2EventHandler(ILogger<Consumer2EventHandler> logger) : IHandleMessages<Consumer2Contract>
{
    public Task Handle(Consumer2Contract message, IMessageHandlerContext context)
    {
        logger.LogInformation(message.Consumer2Property);
        return Task.CompletedTask;
    }
}