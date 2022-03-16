using MassTransit;
using Messages.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MTEndpoint
{
    #region MassTransitConsumer

    public class MessageConsumer : IConsumer<MassTransitEvent>
    {
        readonly ILogger<MessageConsumer> logger;

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<MassTransitEvent> context)
        {
            logger.LogInformation("Received Text: {Text}", context.Message.Text);

            return Task.CompletedTask;
        }
    }

    #endregion
}
