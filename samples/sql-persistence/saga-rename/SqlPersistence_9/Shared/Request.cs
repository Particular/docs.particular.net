using System;
using NServiceBus;

namespace Shared;

public record Request : IMessage
{
    public Guid TheId { get; set; }
}