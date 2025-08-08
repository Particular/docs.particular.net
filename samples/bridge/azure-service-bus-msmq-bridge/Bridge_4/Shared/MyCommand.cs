using NServiceBus;

namespace Shared;

public class MyCommand :
    IMessage
{
    public string Property { get; set; }
}