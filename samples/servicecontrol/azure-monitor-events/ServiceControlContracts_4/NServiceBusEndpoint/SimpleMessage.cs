using System;
using NServiceBus;

public class SimpleMessage :
    IMessage
{
    public Guid Id { get; set; }
}