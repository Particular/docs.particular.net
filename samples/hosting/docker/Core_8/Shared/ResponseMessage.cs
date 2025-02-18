using System;
using NServiceBus;

namespace Shared;

public class ResponseMessage
    : IMessage
{
    public Guid Id { get; set; }
    public string Data { get; set; }
}