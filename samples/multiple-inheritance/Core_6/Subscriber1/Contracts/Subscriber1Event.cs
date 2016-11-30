namespace Subscriber1.Contracts
{
    using NServiceBus;
    public interface Subscriber1Event : 
        IEvent
    {
        string Subscriber1Property { get; set; }
    }
}