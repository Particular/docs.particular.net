using System;
using NServiceBus;

public record CompleteSagaMessage : IMessage
{
    public required Guid TheId { get; init; }
}