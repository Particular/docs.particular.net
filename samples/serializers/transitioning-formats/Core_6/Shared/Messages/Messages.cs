using System.Collections.Generic;
using NServiceBus;
#region messages

public class Order :
    IMessage
{
    public int OrderId { get; set; }
    public Dictionary<int, OrderItem> OrderItems { get; set; }
}

public class OrderItem
{
    public int Quantity { get; set; }
}

#endregion