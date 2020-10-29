using NServiceBus;

namespace Messages
{
    public interface CustomerHasBecomePreferred : IEvent
    {
        string CustomerId { get; set; }
    }
}