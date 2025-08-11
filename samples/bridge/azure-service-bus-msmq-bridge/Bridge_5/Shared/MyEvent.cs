using NServiceBus;

namespace Shared;

public class MyEvent :
    IEvent
{
    public string Property { get; set; }
}