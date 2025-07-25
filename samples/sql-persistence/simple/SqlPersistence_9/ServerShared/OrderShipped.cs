using System;
using NServiceBus;

namespace ServerShared;

public record OrderShipped : IEvent
{
    public required Guid Id { get; set; }
    public required DateTime ShippingDate { get; set; }
}