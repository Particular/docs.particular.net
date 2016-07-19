namespace Core3.PubSub
{
    using NServiceBus;

    public class UserCreatedEvent :
        IEvent
    {
        public string Name { get; set; }
    }
}