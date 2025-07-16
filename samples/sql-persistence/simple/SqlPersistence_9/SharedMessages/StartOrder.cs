using System;
using NServiceBus;

namespace SharedMessages;

public record StartOrder : ICommand
{
    public required Guid OrderId { get; set; }
}