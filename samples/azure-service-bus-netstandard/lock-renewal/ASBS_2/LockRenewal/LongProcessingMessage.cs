using System;
using NServiceBus;

namespace LockRenewal
{
    public class LongProcessingMessage : IMessage
    {
        public TimeSpan ProcessingDuration { get; set; }
    }
}