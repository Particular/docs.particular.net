using System;
using NServiceBus;

namespace Shared;

public record Reply : IMessage
{
    public Guid TheId { get; set; }
    public string OriginatingSagaType { get; set; }
}