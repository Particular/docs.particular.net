using System;
using NServiceBus;

namespace Messages
{

    public class ClientOrder : IMessage
    {
        public Guid OrderId { get; set; }
    }

    public class ClientOrderAccepted : IMessage
    {
        public Guid OrderId { get; set; }
    }

}
