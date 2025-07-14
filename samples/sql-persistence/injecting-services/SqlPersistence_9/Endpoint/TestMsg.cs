using NServiceBus;
using System;

namespace Endpoint;

public record TestMsg : ICommand
{
    public required Guid Id { get; set; }
}