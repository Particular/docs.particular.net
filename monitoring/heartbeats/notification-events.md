---
title: Heartbeat notification events
summary: 
reviewed: 2018-01-05
component: Heartbeats
versions: 'Heartbeats:*'
---

ServiceControl exposes two integration events related to the Heartbeats plugin.

For information about how to subscribe to ServiceControl integration events, see [Using ServiceControl events](/servicecontrol/contracts.md).


## `HeartbeatStopped`

The `HeartbeatStopped` event is published if the ServiceControl instance does not receive a heartbeat from an active endpoint instance within a [configured grace period](/servicecontrol/creating-config-file.md#plugin-specific-servicecontrolheartbeatgraceperiod).

TODO: Put a sample of the event in here


## `HeartbeatRestored`

The `HeartbeatRestored` event is published is the ServiceControl instance starts receiving heartbeats from a previously inactive endpoint instance.

TODO: Put a sample of the event in here