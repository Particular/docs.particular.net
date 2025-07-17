using System;
using System.Runtime.Serialization;
using Azure;
using Azure.Data.Tables;

public class OrderShippingInformation : ITableEntity
{
    [IgnoreDataMember] // makes sure the property doesn't get serialized
    public Guid Id
    {
        get => Guid.Parse(RowKey);
        set => RowKey = value.ToString();
    }

    public Guid OrderId { get; set; }

    public DateTimeOffset ShippedAt { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}