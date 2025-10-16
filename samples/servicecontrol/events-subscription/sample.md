---
title: Monitor with ServiceControl events
summary: A sample showing how to monitor events in ServiceControl
component: ServiceControlContracts
reviewed: 2025-08-28
related:
 - servicecontrol
 - servicecontrol/contracts
 - samples/servicecontrol/monitoring3rdparty
---

This sample shows how to monitor heartbeat and failed message events in ServiceControl, as well as observing the same activity in ServicePulse. The sample uses the [Learning Transport](/transports/learning/) and a portable version of the Particular Service Platform tools. Installing ServiceControl is **not** required.

downloadbutton


## Running the project

Running the project will result in 3 console windows:

1. **NServiceBusEndpoint**: The endpoint that represents the system being monitored.
1. **EndpointsMonitor**: The endpoint that subscribes to ServiceControl heartbeat and failed message events.
1. **PlatformLauncher**: Runs an in-process version of ServiceControl and ServicePulse. When the ServiceControl instance is ready, a browser window will be launched displaying the ServicePulse Dashboard.

The project handles two kinds of events:

### MessageFailed event

A `MessageFailed` event is emitted when processing a message fails and the message is moved to the error queue.

To observe this in action, press <kbd>Enter</kbd> in the `NServiceBusEndpoint` console window to send a new `SimpleMessage` event. Processing of the message fails every time.

> [!NOTE]
> The exception will cause the debugger to enter a breakpoint. It may be preferable to detach the debugger in order to better observe what's going on.

When a `MessageFailed` event is received, the `EndpointsMonitor` prints the following message in its console window:

```
> Received ServiceControl 'MessageFailed' event for a SimpleMessage with ID 42f25e40-a673-61f3-a505-c8dee6d16f8a
```

Using the details in the `MessageFailed` message, handler code can be written to notify operations or development staff by email or other method.

The failed message can also be viewed in the ServicePulse browser window. Navigating to the failed message allows viewing more details about the message failure.


### HeartbeatStopped and HeartbeatRestored events

The `HeartbeatStopped` event is emitted whenever an endpoint fails to send a control message within the expected interval. The `HeartbeatRestored` event is emitted whenever the endpoint successfully sends a control message again.

> [!NOTE]
> The monitor must receive at least one control message before it can observe that the endpoint stopped responding.

To observe this in action, stop the `NServiceBusEndpoint` application and wait up to 30 seconds. When a `HeartbeatStopped` event is received, the `EndpointsMonitor` prints the following message in its console window:

> `Heartbeat from NServiceBusEndpoint stopped.`

Next, restart the `NServiceBusEndpoint` application and wait up to 30 seconds. When a `HeartbeatRestored` event is received, the `EndpointsMonitor` prints the following message in its console window:

> `Heartbeat from EndpointsMonitoring.NServiceBusEndpoint restored.`


## Code walk-through


### NServiceBusEndpoint

Retries are disabled in the sample for simplicity; messages are immediately moved to the error queue after a processing failure:

snippet: DisableRetries

The `MessageFailed` event is published whenever ServiceControl detects a new message in the error queue.

In order to receive `HeartbeatStopped` and `HeartbeatRestored` events, the endpoint must use the [heartbeats plugin](/monitoring/heartbeats).

> [!NOTE]
> Heartbeat control messages are sent [every 10 seconds by default](/monitoring/heartbeats/install-plugin.md#heartbeat-interval) so there will be up to a 30 second delay before ServiceControl realizes that it lost or restored connection with the endpoint.


### EndpointsMonitor

In order to get notifications when the exposed ServiceControl events occur, create an NServiceBus endpoint. Next, reference the `ServiceControl.Contracts` NuGet package and implement a handler which handles ServiceControl events:

snippet: ServiceControlEventsHandlers

> [!IMPORTANT]
> In order to prevent infinite message loops (i.e. processing of an integration event fails -> the faulting message is moved to the ServiceControl error queue, which triggers yet another integration event) the monitoring endpoint must use a dedicated error queue, separate from the ServiceControl one. 

snippet: ServiceControlEventsMonitorCustomErrorQueue

## Notes on other transports

This sample uses the [Learning Transport](/transports/learning/) in order to be portable with no transport dependencies.

When changing this sample to use Azure Service Bus as a transport, note that the subscribing endpoint must not override the default name shortening strategy. The [entity creation configuration settings](/transports/azure-service-bus/configuration.md#entity-creation) of [Azure Service Bus Transport](/transports/azure-service-bus/) should not be changed.

The same applies to [Azure Storage Queues](/transports/azure-storage-queues)' name [sanitization strategy](/transports/azure-storage-queues/sanitization.md#backward-compatibility-with-versions-7-and-below).
