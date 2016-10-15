using System;
using System.Collections.Generic;
using NServiceBus;

public class CreateOrder :
    IMessage
{
    public int OrderId;
    public DateTime Date;
    public int CustomerId;
    public List<OrderItem> OrderItems;
}