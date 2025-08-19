using System;
using NServiceBus;

namespace Shared;

public record StartTimeoutSaga : IMessage
{
    public Guid TheId { get; set; }
}