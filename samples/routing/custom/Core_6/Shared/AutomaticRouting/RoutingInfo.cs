using System;

class RoutingInfo
{
    public string EndpointName { get; set; }
    public string TransportAddress { get; set; }
    public string[] HandledEventTypes { get; set; } = new string[0];
    public string[] HandledCommandTypes { get; set; } = new string[0];
    public bool Active { get; set; }
    public DateTime Timestamp { get; set; }
}
