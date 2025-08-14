namespace Messages
{
    using System;
    using NServiceBus;

    public class ClientOrderResponse :
        IMessage
    {
        public Guid OrderId { get; set; }
    }
}