using System;
using NServiceBus;

namespace Receiver
{
    using Shared;

    #region NativeMessageHandler

    public class NativeMessageHandler : IHandleMessages<NativeMessage>
    {
        public void Handle(NativeMessage message)
        {
            Console.WriteLine($"Message content: {message.Content}");
            Console.WriteLine($"Received native message sent on {message.SendOnUtc} UTC");
        }
    }

    #endregion

}