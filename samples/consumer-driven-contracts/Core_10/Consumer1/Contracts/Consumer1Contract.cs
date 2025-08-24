namespace Subscriber1.Contracts;

using NServiceBus;

public interface Consumer1Contract : IEvent
{
    string Consumer1Property { get; set; }
}