using NServiceBus;

namespace Receiver;

public record LegacyOrderDetected : IMessage
{
    public required string OrderId { get; set; }
}