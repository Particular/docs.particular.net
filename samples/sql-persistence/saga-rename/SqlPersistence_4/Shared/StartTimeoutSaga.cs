using System;
using NServiceBus;

public class StartTimeoutSaga:
    IMessage
{
    public Guid TheId { get; set; }
}