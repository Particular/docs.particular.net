using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Tracing
{
    public class MessageHandler : IHandleMessages<InitialCommand>, IHandleMessages<FollowupEvent>
    {
        ILogger<MessageHandler> logger;

        public MessageHandler(ILogger<MessageHandler> logger)
        {
            this.logger = logger;
        }

        public async Task Handle(InitialCommand message, IMessageHandlerContext context)
        {
            if (new Random().Next(1, 4) == 1)
            {
                throw new Exception("Boom!");
            }

            await Task.Delay(TimeSpan.FromSeconds(2));

            logger.LogInformation($"Message {nameof(InitialCommand)} successfully processed.");
            
            await context.Publish<FollowupEvent>();
        }

        public async Task Handle(FollowupEvent message, IMessageHandlerContext context)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            logger.LogInformation($"Message {nameof(FollowupEvent)} successfully processed.");
        }
    }
}