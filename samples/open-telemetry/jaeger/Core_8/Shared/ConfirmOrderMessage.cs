namespace Shared;

public class ConfirmOrderMessage : IMessage
{
    public Guid OrderId { get; set; }
}