using NServiceBus;

namespace Orders.Events
{
    using System;

    public class OrderPlaced :IEvent
    {
        public Guid OrderId { get; set; }
    }
}
