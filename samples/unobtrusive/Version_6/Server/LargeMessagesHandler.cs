namespace Server
{
    using System;
    using System.Threading.Tasks;
    using Messages;
    using NServiceBus;

    public class LargeMessagesHandler : IHandleMessages<LargeMessage>
    {
        public Task Handle(LargeMessage message, IMessageHandlerContext context)
        {
            if (message.LargeDataBus == null)
            {
                Console.WriteLine("Message [{0}] received, id:{1}", message.GetType(), message.RequestId);
            }
            else
            {
                Console.WriteLine("Message [{0}] received, id:{1} and payload {2} bytes", message.GetType(), message.RequestId, message.LargeDataBus.Length);
            }
            return Task.FromResult(0);
        }
        
    }
}