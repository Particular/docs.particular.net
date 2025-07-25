using System;
using NServiceBus;

public record MyMessage : IMessage
{
    public required Guid Id { get; init; }
}
