namespace Subscriber2.Contracts;

using NServiceBus;

public interface Consumer2Contract : IEvent
{
    string Consumer2Property { get; set; }
}