using System;
using NServiceBus;

namespace Messages
{
    public class OrderReceived :
        IEvent
    {
        public Guid OrderId { get; set; }
    }
}