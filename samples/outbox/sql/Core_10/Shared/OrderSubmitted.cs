using System;
using NServiceBus;

public record OrderSubmitted(Guid OrderId, decimal Value) : IEvent;