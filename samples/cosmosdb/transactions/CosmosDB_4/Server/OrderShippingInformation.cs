using System;
using Newtonsoft.Json;

public class OrderShippingInformation
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public DateTimeOffset ShippedAt { get; set; }
}