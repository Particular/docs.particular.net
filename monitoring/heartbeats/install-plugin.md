---
title: Install Heartbeats Plugin
summary: Enabling endpoint instance monitoring by installing the Heartbeats plugin
reviewed: 2018-01-05
component: Heartbeats
versions: 'Heartbeats:*'
---

To install the Heartbeats plugin into an endpoint add the following to the endpoint configuration:

snippet: HeartbeatsNew_Enable

WARN: When installing the heartbeat plugin, also configure each instance with a deterministic [Host Identifier](/nservicebus/hosting/override-hostid.md). This identifier is used to keep track of which instance is sending heartbeat messages to the ServiceControl instance.


### Heartbeat interval

Heartbeat messages are sent by the plugin, at a predefined interval of 10 seconds. As shown above, the interval value can be overridden on a per endpoint basis.

When configuring heartbeat interval, ensure ServiceContol setting [`HeartbeatGracePeriod`](/servicecontrol/creating-config-file.md#plugin-specific-servicecontrolheartbeatgraceperiod) is greater than the heartbeat interval. 


### Time-To-Live (TTL)

When the plugin sends heartbeat messages, the default TTL is fixed to four times the value of the Heartbeat interval. As shown above, the interval value can be overridden on a per endpoint basis.

WARN: See [expired heartbeats](expired-heartbeats.md) for more information about what happens to expired heartbeats.