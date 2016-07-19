namespace Messages
{
    using System;
    using NServiceBus;

    public class ClientOrderAccepted :
        IMessage
    {
        public Guid OrderId { get; set; }
    }
}