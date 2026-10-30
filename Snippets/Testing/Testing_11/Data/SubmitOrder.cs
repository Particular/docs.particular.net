using System;
using NServiceBus;

public class SubmitOrder :
    IMessage
{
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
}