using NServiceBus;

namespace Shared;

public class Pong : IMessage
{
    public required string Acknowledgement { get; set; }
}