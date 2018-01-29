---
title: Heartbeat notification events
summary:
reviewed: 2018-01-26
component: Heartbeats
versions: 'Heartbeats:*'
---

ServiceControl exposes two integration events related to the Heartbeats plugin.

For information about how to subscribe to ServiceControl integration events, see [Using ServiceControl events](/servicecontrol/contracts.md).


## `HeartbeatStopped`

The `HeartbeatStopped` event is published if the ServiceControl instance does not receive a heartbeat from an active endpoint instance within a [configured grace period](/servicecontrol/creating-config-file.md#plugin-specific-servicecontrolheartbeatgraceperiod).

```csharp
public class HeartbeatStopped
{
    /// <summary>
    /// The date and time last hearbeat has been received from the endpoint.
    /// </summary>
    public DateTime LastReceivedAt { get; set; }

    /// <summary>
    /// The date and time the lack of heartbeat been detected by ServiceControl.
    /// </summary>
    public DateTime DetectedAt { get; set; }

    /// <summary>
    /// The name of the endpoint
    /// </summary>
    public string EndpointName { get; set; }

    /// <summary>
    /// The unique identifier for the host that runs the endpoint
    /// </summary>
    public Guid HostId { get; set; }

    /// <summary>
    /// The name of the host
    /// </summary>
    public string Host { get; set; }
}
```


## `HeartbeatRestored`

The `HeartbeatRestored` event is published is the ServiceControl instance starts receiving heartbeats from a previously inactive endpoint instance.

```csharp
public class HeartbeatRestored
{
    /// <summary>
    /// The date and time the heartbeat been again detected by ServiceControl.
    /// </summary>
    public DateTime RestoredAt { get; set; }

    /// <summary>
    /// The name of the endpoint
    /// </summary>
    public string EndpointName { get; set; }

    /// <summary>
    /// The unique identifier for the host that runs the endpoint
    /// </summary>
    public Guid HostId { get; set; }

    /// <summary>
    /// The name of the host
    /// </summary>
    public string Host { get; set; }
}
```
