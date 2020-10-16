using System;

public class ValidateOrder :
    IMessage
{
    public Guid OrderId { get; set; }
    public string Sender { get; set; }
}