using System;
using NServiceBus;

public record MyMessage : ICommand
{
    public required Guid Id { get; init; }
}
