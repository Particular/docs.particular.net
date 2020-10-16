using System;

public class OrderValidated :
    IMessage
{
    public Guid OrderId { get; set; }
    public string Sender { get; set; }
}