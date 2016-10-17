using System.Collections.Generic;
using NServiceBus;

public class CreateOrder :
    IMessage
{
    public int OrderId { get; set; }
    public Dictionary<int,OrderItem> OrderItems { get; set; }
}