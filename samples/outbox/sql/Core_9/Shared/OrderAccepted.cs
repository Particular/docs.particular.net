using System;
using NServiceBus;

public record OrderAccepted(Guid OrderId) : IMessage;