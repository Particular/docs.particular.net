using Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace NServiceBusSubscriber
{
    public class MassTransitEventHandler : IHandleMessages<MassTransitEvent>,
        IHandleMessages<IMTEvent>
    {
        public Task Handle(MassTransitEvent message, IMessageHandlerContext context)
        {
            logger.Info($"Received Text: {message.Text}");
            return Task.CompletedTask;
        }

        public Task Handle(IMTEvent message, IMessageHandlerContext context)
        {
            logger.Info("IMTEvent received too");
            return Task.CompletedTask;
        }

        static ILog logger = LogManager.GetLogger<MassTransitEventHandler>();
    }
}
