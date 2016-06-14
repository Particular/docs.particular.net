using System;
using System.Collections.Generic;

class RoutingInfo
{
    public string EndpointName { get; set; }
    public string Discriminator { get; set; }
    public Dictionary<string, string> InstanceProperties { get; set; }
    public string[] HandledMessageTypes { get; set; }
    public bool Active { get; set; }
    public DateTime Timestamp { get; set; }
}
