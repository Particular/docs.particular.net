---
title: Install Heartbeats Plugin
summary: Enabling endpoint instance monitoring by installing the Heartbeats plugin
reviewed: 2018-07-30
component: Heartbeats
versions: 'Heartbeats:*'
---

To install the Heartbeats plugin into an endpoint, reference the [NServiceBus.Heartbeats NuGet package](https://www.nuget.org/packages/NServiceBus.Heartbeat/) and add the following to the endpoint configuration:

snippet: HeartbeatsNew_Enable

NOTE: `ServiceControl_Queue` is a placeholder for the actual ServiceControl input queue. The ServiceControl input queue is equal to the [ServiceControl service name](/servicecontrol/installation.md#service-name-and-plugins) as configured in the ServiceControl Management tool.


### Heartbeat interval

Heartbeat messages are sent by the plugin at a predefined interval of 10 seconds. As shown above, the interval value can be overridden on a per-endpoint basis.

NOTE: When configuring the heartbeat interval, ensure the ServiceContol setting [`HeartbeatGracePeriod`](/servicecontrol/creating-config-file.md#plugin-specific-servicecontrolheartbeatgraceperiod) is greater than the heartbeat interval.


### Time-To-Live (TTL)

When the plugin sends heartbeat messages, the default TTL is fixed to four times the value of the heartbeat interval. As shown above, the interval value can be overridden on a per-endpoint basis. See [Expired heartbeats](expired-heartbeats.md) for more information about what happens to expired heartbeats.


### Identifying scaled-out endpoints

When installing the heartbeat plugin on a scaled-out endpoint, also configure each instance with a deterministic [Host Identifier](/nservicebus/hosting/override-hostid.md). This identifier is used to keep track of which instance is sending heartbeat messages to the ServiceControl instance.
