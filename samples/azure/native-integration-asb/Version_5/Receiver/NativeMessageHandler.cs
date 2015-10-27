using System;
using NServiceBus;

namespace Receiver
{
    using Shared;

    public class NativeMessageHandler : IHandleMessages<NativeMessage>
    {
        #region NativeMessageHandler

        public void Handle(NativeMessage message)
        {
            Console.WriteLine($"Message content: {message.Content}");
            Console.WriteLine($"Received native message sent on {message.SendOnUtc} UTC");
        }

        #endregion
    }
}