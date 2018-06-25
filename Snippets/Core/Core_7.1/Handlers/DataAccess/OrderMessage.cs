namespace Core7.Handlers.DataAccess
{
    using NServiceBus;

    public class OrderMessage :
        IMessage
    {
        public object OrderId { get; set; }
    }
}