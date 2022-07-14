namespace Shared
{
    using System;
    using NServiceBus;

    public class DeclineOrderMessage : IMessage
    {
        public Guid OrderId { get; set; }
    }
}