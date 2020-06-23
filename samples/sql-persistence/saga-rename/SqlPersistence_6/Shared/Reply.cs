using System;
using NServiceBus;

public class Reply:
    IMessage
{
    public Guid TheId { get; set; }
    public string OriginatingSagaType { get; set; }
}