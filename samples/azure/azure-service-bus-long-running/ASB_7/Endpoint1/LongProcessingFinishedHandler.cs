namespace Endpoint1
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using Shared;

    public class LongProcessingFinishedHandler : IHandleMessages<LongProcessingFinished>
    {
        static ILog log = LogManager.GetLogger<LongProcessingFinishedHandler>();

        public Task Handle(LongProcessingFinished message, IMessageHandlerContext context)
        {
            log.Info($"Request with ID {message.Id} has successfully finished.");

            return Task.FromResult(0);
        }
    }
}