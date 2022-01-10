using NServiceBus;

namespace Messages
{
    public interface CustomerHasBecomeNonPreferred : IEvent
    {
        string CustomerId { get; set; }
    }
}