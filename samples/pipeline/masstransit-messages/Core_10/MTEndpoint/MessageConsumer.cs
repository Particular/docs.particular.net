using System.Threading.Tasks;
using MassTransit;
using Messages.Events;
using Microsoft.Extensions.Logging;

namespace MTEndpoint
{
    #region MassTransitConsumer

    public class MessageConsumer(ILogger<MessageConsumer> logger) : IConsumer<MassTransitEvent>
    {
        public Task Consume(ConsumeContext<MassTransitEvent> context)
        {
            logger.LogInformation("Received Text: {Text}", context.Message.Text);

            return Task.CompletedTask;
        }
    }

    #endregion
}
