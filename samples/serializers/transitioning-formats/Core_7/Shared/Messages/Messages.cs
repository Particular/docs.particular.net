using System.Collections.Generic;
using NServiceBus;
#region messages

public class Order :
    IMessage
{
    public int OrderId { get; set; }

    // Where the dictionary key is the id of the order line item
    public Dictionary<int, OrderItem> OrderItems { get; set; }
}

public class OrderItem
{
    public int Quantity { get; set; }
}

#endregion