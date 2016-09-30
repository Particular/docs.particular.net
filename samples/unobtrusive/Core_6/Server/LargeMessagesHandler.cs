namespace Server
{
    using System.Threading.Tasks;
    using Messages;
    using NServiceBus;
    using NServiceBus.Logging;

    public class LargeMessagesHandler :
        IHandleMessages<LargeMessage>
    {
        static ILog log = LogManager.GetLogger<LargeMessagesHandler>();

        public Task Handle(LargeMessage message, IMessageHandlerContext context)
        {
            if (message.LargeDataBus == null)
            {
                log.Info($"Message [{message.GetType()}] received, id:{message.RequestId}");
            }
            else
            {
                log.Info($"Message [{message.GetType()}] received, id:{message.RequestId} and payload {message.LargeDataBus.Length} bytes");
            }
            return Task.CompletedTask;
        }
    }
}