namespace Core.PubSub.Publishing;

using NServiceBus;

public class UserCreatedEvent :
    IEvent
{
    public string Name { get; set; }
}