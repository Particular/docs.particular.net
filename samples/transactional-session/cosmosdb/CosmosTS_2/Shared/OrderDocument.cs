using Newtonsoft.Json;

public class OrderDocument
{
    [JsonProperty("id")]
    public string OrderId { get; set; }

    public string CustomerId { get; set; }

    public string Status { get; set; }
}