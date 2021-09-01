using System;
using NServiceBus;

public class LongRunningMessage :
    IMessage
{
    public Guid DataId { get; set; }
}