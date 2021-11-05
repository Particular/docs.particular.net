using Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace NServiceBusSubscriber
{
    #region NSBMessageHandler
    public class MassTransitEventHandler : IHandleMessages<MassTransitEvent>
    {
        public Task Handle(MassTransitEvent message, IMessageHandlerContext context)
        {
            logger.Info($"Received Text: {message.Text}");
            return Task.CompletedTask;
        }

        static ILog logger = LogManager.GetLogger<MassTransitEventHandler>();
    }
    #endregion
}