using System.Threading.Tasks;
using Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace NServiceBusSubscriber
{
    #region NSBMessageHandler
    public class MassTransitEventHandler (ILogger<MassTransitEventHandler> logger): IHandleMessages<MassTransitEvent>
    {
        public Task Handle(MassTransitEvent message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received Text: {Text}", message.Text);
            return Task.CompletedTask;
        }

    }
    #endregion
}