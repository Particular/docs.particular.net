using NServiceBus;

namespace Orders.Commands
{
    using System;

    public class PlaceOrder:IMessage
    {
        public Guid OrderId { get; set; }
    }
}
