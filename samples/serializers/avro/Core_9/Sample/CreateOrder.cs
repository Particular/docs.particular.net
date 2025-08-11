using System;
using System.Collections.Generic;
using NServiceBus;

public record CreateOrder : IMessage
{
    public int OrderId { get; set; }
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}