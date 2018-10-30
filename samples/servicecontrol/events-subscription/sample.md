---
title: Monitor with ServiceControl events
summary: A sample showing how to monitor events in ServiceControl
component: ServiceControlContracts
reviewed: 2018-04-20
tags:
 - Notifications
related:
 - servicecontrol
 - servicecontrol/contracts
 - servicecontrol/plugins
 - samples/servicecontrol/monitoring3rdparty
---

This sample shows how to monitor heartbeat and failed message events in ServiceControl.

## Prerequisites

 1. [Install ServiceControl](/servicecontrol/installation.md).
 1. Using [ServiceControl Management](/servicecontrol/license.md#servicecontrol-management-app) tool, set up ServiceControl to monitor endpoints using MSMQ transport.
 1. Ensure the `ServiceControl` process is running before running the sample. 

NOTE: When using the [Azure Service Bus transport](/transports/azure-service-bus/), the subscribing endpoint must also use the same name shortening strategy as ServiceControl. See the [configuration settings](/transports/azure-service-bus-netstandard/configuration.md#entity-creation), or if using the [legacy Azure Service Bus transport](), see its [sanitization strategy documentation](/transports/azure-service-bus/sanitization.md)

NOTE: The ServiceControl endpoint queue name configured in `EndpointsMonitor` must match the name of the ServiceControl instance with the underlying transport naming rules.

## Running the project

The project handles two kinds of events:

### MessageFailed event

A `MessageFailed` event is emitted when processing a message fails and the message is moved to the error queue.

To observe this in action, press <kbd>Enter</kbd> in the `NServiceBusEndpoint` console window to send a new `SimpleMessage` event. Processing of the message fails every time.

When a `MessageFailed` event is received, the `EndpointsMonitor` prints the following message in its console window: 

> `Received ServiceControl 'MessageFailed' event for a SimpleMessage.`


### HeartbeatStopped and HeartbeatRestored events

The `HeartbeatStopped` event is emitted whenever an endpoint fails to send a control message at an expected interval. The `HeartbeatRestored` event is emitted whenever the endpoint successfully sends a control message again. 

Note: The monitor must receive at least one control message before it can observe that the endpoint stopped responding.

To observe this in action, stop the `NServiceBusEndpoint` application and wait up to 30 seconds. When a `HeartbeatStopped` event is received, the `EndpointsMonitor` prints the following message in its console window:

> `Heartbeat from NServiceBusEndpoint stopped.`

Next, restart the `NServiceBusEndpoint` application and wait up to 30 seconds. When a `HeartbeatRestored` event is received, the `EndpointsMonitor` prints the following message in its console window:

> `Heartbeat from EndpointsMonitoring.NServiceBusEndpoint restored.`


## Code walk-through 

The solution consists of two projects: `NServicebusEndpoint` and `EndpointsMonitor`. `NServiceBusEndpoint` is a simple endpoint which is monitored by the `EndpointsMonitor`.


### NServiceBusEndpoint

Retries are disabled in the sample for simplicity; messages are immediately moved to the error queue after a processing failure:

snippet: DisableRetries

The `MessageFailed` event is published for any standard NServiceBus endpoint that is monitored by ServiceControl.

In order to receive `HeartbeatStopped` and `HeartbeatRestored` events, the endpoint must use the [heartbeats plugin](/monitoring/heartbeats).

NOTE: Heartbeat control messages are sent [every 30 seconds by default](/monitoring/heartbeats/legacy#configuration-time-to-live-ttl) so there will be up to a 30 second delay before ServiceControl realizes that it lost or restored connection with the endpoint.


### EndpointsMonitor

In order to get notifications when the exposed ServiceControl events occur, create an NServiceBus endpoint. Next, reference the `ServiceControl.Contracts` NuGet package and implement a handler which handles ServiceControl events:

snippet: ServiceControlEventsHandlers
