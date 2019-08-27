using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace AzureFunctions.ASQTrigger
{
    public class TriggerMessageHandler : IHandleMessages<TriggerMessage>
    {
        private static readonly ILog Log = LogManager.GetLogger<TriggerMessageHandler>();

        public Task Handle(TriggerMessage message, IMessageHandlerContext context)
        {
            Log.Info($"Handling {nameof(TriggerMessage)} in {nameof(TriggerMessageHandler)}");
            return context.SendLocal(new FollowupMessage());
        }
    }
}