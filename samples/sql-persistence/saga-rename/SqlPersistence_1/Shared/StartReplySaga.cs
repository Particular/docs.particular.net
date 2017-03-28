using System;
using NServiceBus;

public class StartReplySaga:
    IMessage
{
    public Guid TheId { get; set; }
}