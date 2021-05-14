---
title: Monitor ServiceControl events with Azure Application Insights
summary: A sample showing how to monitor events published by ServiceControl with Azure Application Insights
component: ServiceControlContracts
reviewed: 2021-04-16
related:
 - servicecontrol
 - servicecontrol/contracts
 - servicecontrol/plugins
 - samples/servicecontrol/monitoring3rdparty
---

This sample shows how to monitor a running NServiceBus system with ServiceControl and ServicePulse, as well as how to integrate with existing monitoring solutions. The sample uses the [learning transport](/transports/learning/) and a portable version of the Particular Service Platform tools. Installing ServiceControl is **not** required.

include: platformlauncher-windows-required

downloadbutton

## Running the project

Running the project shows three console windows:

1. **NServiceBusEndpoint**: The endpoint that represents the system being monitored.
1. **AzureMonitorConnector**: The endpoint that subscribes to ServiceControl notification events and pushes them to Application Insights as custom telemetry events.
1. **PlatformLauncher**: Runs an in-process version of ServiceControl and ServicePulse. When the ServiceControl instance is ready, a browser window is launched displaying the ServicePulse dashboard.

The samples triggers two types of events:

### Message failures

A `MessageFailed` event is emitted when processing a message fails and the message is moved to the error queue.

To observe this in action, press <kbd>Enter</kbd> in the `NServiceBusEndpoint`console window. The application will generate a new `SimpleMessage` event that fails when processed.

NOTE: The exception will cause the debugger to enter a breakpoint. Detach the debugger in order to better observe what's going on.

When a `MessageFailed` event is received, the `AzureMonitorConnector` prints the following message in its console window:

```
> Received ServiceControl 'MessageFailed' event for a SimpleMessage with ID 42f25e40-a673-61f3-a505-c8dee6d16f8a
```

The failed message can also be viewed in the ServicePulse browser window. Navigating to the failed message shows more details about the message failure.

### Heartbeat statuses

The `HeartbeatStopped` event is published whenever an endpoint fails to send a control message within an expected interval. The `HeartbeatRestored` event is published whenever the endpoint successfully sends a control message again.

Note: The monitor must receive at least one control message before it can observe that the endpoint stopped responding.

To observe this in action, stop the `NServiceBusEndpoint` process and wait up to 30 seconds. When a `HeartbeatStopped` event is received, the `AzureMonitorConnector` prints the following message to the console window:

> `Heartbeat from NServiceBusEndpoint stopped.`

Next, restart the `NServiceBusEndpoint` application and wait up to 30 seconds. When a `HeartbeatRestored` event is received, the `AzureMonitorConnector` prints the following message in its console window:

> `Heartbeat from EndpointsMonitoring.NServiceBusEndpoint restored.`

## Code walk-through

### NServiceBusEndpoint

Retries are disabled in the sample for simplicity; messages are immediately moved to the error queue after a processing failure:

snippet: DisableRetries

The `MessageFailed` event is published whenever ServiceControl detects a new message in the error queue.

In order to receive `HeartbeatStopped` and `HeartbeatRestored` events, the endpoint must use the [heartbeats plugin](/monitoring/heartbeats).

NOTE: Heartbeat control messages are sent [every 30 seconds by default](/monitoring/heartbeats/legacy#configuration-time-to-live-ttl) so there will be up to a 30 second delay before ServiceControl realizes that it lost or restored connection with the endpoint.

## Connect to Application Insights Azure Monitor

To connect the sample code to Application Insights, the instrumentation key must be provided. The key is loaded from the `ApplicationInsightKey` environment variable.

The instrumentation key can be retrieved from the Azure Portal by locating the Application Insights instance, then navigating to the Properties view.

snippet: AppInsightsSdkSetup

### AzureMonitorConnector

In order to get notifications when the exposed ServiceControl events occur, create an NServiceBus endpoint. Next, reference the `ServiceControl.Contracts` NuGet package and implement a handler which handles specific ServiceControl events:

snippet: AzureMonitorConnectorEventsHandler

The handler creates a custom telemetry event and pushes it to Application Insights.

## Notes on other transports

This sample uses the [learning transport](/transports/learning/) in order to be portable with no transport dependencies.

When adjusting this sample to use the [Azure Service Bus transport](/transports/azure-service-bus/), note that the subscribing endpoint must also use the same name shortening strategy as ServiceControl. See the [configuration settings](/transports/azure-service-bus/configuration.md#entity-creation), or if using the [legacy Azure Service Bus transport](/transports/azure-service-bus/legacy/), see its [sanitization strategy documentation](/transports/azure-service-bus/legacy/sanitization.md). 

The same applies to the [Azure Storage Queues](/transports/azure-storage-queues) name [sanitization strategy](/transports/azure-storage-queues/sanitization.md#backward-compatibility-with-versions-7-and-below)
