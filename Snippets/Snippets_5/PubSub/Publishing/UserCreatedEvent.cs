namespace Snippets5.PubSub.Publishing
{
    using NServiceBus;

    public class UserCreatedEvent : IEvent
    {
        public string Name { get; set; }
    }
}