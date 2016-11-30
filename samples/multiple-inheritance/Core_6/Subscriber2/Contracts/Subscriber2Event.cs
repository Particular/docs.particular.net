namespace Subscriber2.Contracts
{
    using NServiceBus;
    public interface Subscriber2Event : 
        IEvent
    {
        string Subscriber2Property { get; set; }
    }
}