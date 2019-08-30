using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace AwsLambda.Console
{
    public class BackToSenderMessageHandler : IHandleMessages<BackToSenderMessage>
    {
        static readonly ILog Log = LogManager.GetLogger<BackToSenderMessageHandler>();

        public Task Handle(BackToSenderMessage message, IMessageHandlerContext context)
        {
            Log.Info($"Handling {nameof(BackToSenderMessage)} in {nameof(BackToSenderMessageHandler)}");
            return Task.CompletedTask;
        }
    }
}