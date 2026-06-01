namespace Shared;

using NServiceBus;

public class ConfirmOrderMessage : IMessage
{
    public Guid OrderId { get; set; }
}