using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Tracing
{
    public class MessageHandler : IHandleMessages<InitialCommand>, IHandleMessages<FollowupEvent>
    {
        public async Task Handle(InitialCommand message, IMessageHandlerContext context)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            await context.Publish<FollowupEvent>();
        }

        public async Task Handle(FollowupEvent message, IMessageHandlerContext context)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}