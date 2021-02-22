using System;
using NServiceBus;

namespace Endpoint
{
    public class LongProcessingMessage : IMessage
    {
        public TimeSpan ProcessingDuration { get; set; }
    }
}