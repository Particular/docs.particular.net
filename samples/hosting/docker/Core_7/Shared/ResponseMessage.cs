using NServiceBus;
using System;

public class ResponseMessage
    : IMessage
{
    public Guid Id { get; set; }
    public string Data { get; set; }
}