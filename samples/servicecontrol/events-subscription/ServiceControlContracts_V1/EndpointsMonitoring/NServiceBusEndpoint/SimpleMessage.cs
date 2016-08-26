using System;
using NServiceBus;

namespace NServiceBusEndpoint
{
    public class SimpleMessage : IMessage
    {
        public Guid Id { get; set; }
    }
}
