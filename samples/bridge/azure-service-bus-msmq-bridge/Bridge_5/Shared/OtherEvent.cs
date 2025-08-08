using NServiceBus;

namespace Shared;

public class OtherEvent :
    IEvent
{
    public string Property { get; set; }
}