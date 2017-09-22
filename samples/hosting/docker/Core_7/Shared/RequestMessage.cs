using NServiceBus;
using System;

public class RequestMessage
    : IMessage
{
    public Guid Id { get; set; }
    public string Data { get; set; }
}