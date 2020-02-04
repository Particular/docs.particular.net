using System;
using NServiceBus;

public class Request:
    IMessage
{
    public Guid TheId { get; set; }
}