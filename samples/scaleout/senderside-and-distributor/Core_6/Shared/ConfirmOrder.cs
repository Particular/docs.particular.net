using System;

public class ConfirmOrder
    : IMessage
{
    public Guid OrderId { get; set; }
}