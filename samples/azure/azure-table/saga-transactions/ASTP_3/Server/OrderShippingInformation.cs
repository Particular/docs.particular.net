using System;
using Microsoft.Azure.Cosmos.Table;

public class OrderShippingInformation : TableEntity
{
    [IgnoreProperty]
    public Guid Id
    {
        get => Guid.Parse(RowKey);
        set => RowKey = value.ToString();
    }

    public Guid OrderId { get; set; }

    public DateTimeOffset ShippedAt { get; set; }
}