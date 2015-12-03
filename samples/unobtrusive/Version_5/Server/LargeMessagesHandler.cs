namespace Server
{
    using System;
    using Messages;
    using NServiceBus;

    public class LargeMessagesHandler : IHandleMessages<LargeMessage>
    {
        public void Handle(LargeMessage message)
        {
            if (message.LargeDataBus == null)
            {
                Console.WriteLine("Message [{0}] received, id:{1}", message.GetType(), message.RequestId);
            }
            else
            {
                Console.WriteLine("Message [{0}] received, id:{1} and payload {2} bytes", message.GetType(), message.RequestId, message.LargeDataBus.Length);
            }
        }
    }
}