using System;
using NServiceBus;

public record StartSagaMessage : IMessage
{
    public required Guid TheId { get; init; }
    public required DateTimeOffset SentTime { get; init; }
}