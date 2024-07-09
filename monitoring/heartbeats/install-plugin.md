---
title: Heartbeat plugin installation
summary: Monitor endpoint instance availability by installing the heartbeat plugin
reviewed: 2023-02-10
component: Heartbeats
versions: 'Heartbeats:*'
---

> [!NOTE]
> This plugin can be enabled and configured with the [ServicePlatform Connector plugin](/platform/connecting.md).

To install the heartbeat plugin in an endpoint, reference the [NServiceBus.Heartbeat NuGet package](https://www.nuget.org/packages/NServiceBus.Heartbeat/) and configure the endpoint to send heartbeats:

snippet: HeartbeatsNew_Enable

> [!NOTE]
> `ServiceControl_Queue` is a placeholder for the name of the ServiceControl input queue. The name of the ServiceControl input queue matches the [ServiceControl service name](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontrolinternalqueuename-servicecontrol-plugins) configured in the ServiceControl Management application.

### Heartbeat interval

The plugin sends heartbeat messages with a default frequency of 10 seconds. As shown above, the frequency may be overridden for each endpoint.

> [!NOTE]
> The frequency must be lower than the [`HeartbeatGracePeriod`](/servicecontrol/servicecontrol-instances/configuration.md#plugin-specific-servicecontrolheartbeatgraceperiod) in ServiceControl.

### Time-To-Live (TTL)

The plugin sends heartbeat messages with a default TTL of four times the frequency. As shown above, the TTL may be overridden for each endpoint. See the documentation for [expired heartbeats](expired-heartbeats.md) for more information.

### Identifying scaled-out endpoints

When the heartbeat plugin is installed in a scaled-out endpoint, each endpoint instance must be configured with a unique [host identifier](/nservicebus/hosting/override-hostid.md). The identifiers are used by ServiceControl to keep track of all instances and to identify which instance sent a given heartbeat message.
