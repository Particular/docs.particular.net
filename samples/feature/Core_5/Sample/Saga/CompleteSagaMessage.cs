using System;
using NServiceBus;

public class CompleteSagaMessage : IMessage
{
    public Guid TheId { get; set; }
}