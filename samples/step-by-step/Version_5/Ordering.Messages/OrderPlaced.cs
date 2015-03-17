namespace Ordering.Messages
{
    using System;
    using NServiceBus;

    public class OrderPlaced : IEvent
    {
        public Guid OrderId { get; set; }
    }
}
