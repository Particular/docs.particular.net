using System;
using NServiceBus;

namespace ServerShared;

public record ShipOrder : ICommand
{
    public required Guid OrderId { get; set; }
}