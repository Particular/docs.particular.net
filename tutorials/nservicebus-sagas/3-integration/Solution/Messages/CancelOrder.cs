using NServiceBus;

namespace Messages
{
    public class CancelOrder : ICommand
    {
        public string OrderId { get; set; }
    }
}