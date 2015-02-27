using System;
using NServiceBus;

namespace Messages
{
    public class OrderAccepted : IMessage
    {
        public string OrderId { get; set; }
    }
}