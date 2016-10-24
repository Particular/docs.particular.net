namespace Core5.Handlers
{
    using NServiceBus;

    public class OrderMessage : IMessage
    {
        public object OrderId { get; set; }
    }
}