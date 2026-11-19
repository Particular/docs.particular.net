using System;
using Newtonsoft.Json;

#region BusinessData
public class ProcessedOrder
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public string MessageId { get; set; } = string.Empty;

    public DateTimeOffset ProcessedAt { get; set; }
}
#endregion
