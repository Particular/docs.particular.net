using System;
using NServiceBus;

public class StartSagaMessage :
    IMessage
{
    public Guid TheId { get; set; }
    public DateTimeOffset SentTime { get; set; }
}