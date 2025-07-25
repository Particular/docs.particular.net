using System;
using NServiceBus;

namespace Shared;

public record StartReplySaga : IMessage
{
    public Guid TheId { get; set; }
}