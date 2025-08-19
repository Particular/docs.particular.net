using NServiceBus;

namespace Receiver;

#region MessageContract

public record PlaceOrder : IMessage
{
    public required string OrderId { get; set; }
}

#endregion