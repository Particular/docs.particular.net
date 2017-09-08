---
title: Using ServiceControl events
summary: Build custom notifications by subscribing to ServiceControl events
reviewed: 2016-10-06
component: ServiceControlContracts
tags:
 - Notifications
redirects:
 - servicepulse/custom-notification-and-alerting-using-servicecontol-events
 - servicepulse/custom-notification-and-alerting-using-servicecontrol-events
 - servicecontrol/external-integrations
related:
 - samples/servicecontrol/events-subscription
---


## Custom notification and alerting using ServiceControl events

ServiceControl events enable the construction of custom notifications and integration that will alert of something going wrong in the system.

WARNING: External notification events are sent in batches. If a problem is encountered part way through a batch, the entire batch will be re-sent. This can result in receiving multiple events for a single notification.

[ServiceControl Endpoint Plugins](/servicecontrol/plugins/) collect information from monitored NServiceBus endpoints.


### Alerting on FailedMessages Event

Once a message ends up in the error queue ServiceControl will publish a `MessageFailed` event. The message contains:

 * The processing of the message (which endpoint sent and received the message).
 * The failure cause (the exception type and message).
 * The message itself (the headers and, if using non-binary serialization, also the body).


### Subscribing to ServiceControl Events

ServiceControl publishes `MessageFailed` event when a message gets to the error queue, it is possible to subscribe to these events and act on them (send an email, pager duty and so on).

To subscribe to the `MessageFailed` event:

 * Create an [NServiceBus endpoint](/nservicebus/hosting/nservicebus-host/).
 * Install the [ServiceControl.Contracts NuGet package](https://www.nuget.org/packages/ServiceControl.Contracts/).
 * Add the message mapping in the `UnicastBusConfig` section of the endpoint's app.config so that this endpoint will subscribe to the events from ServiceControl as shown:

snippet: ServiceControlEventsXmlConfig

 * Customize the endpoint configuration to use `JsonSerializer` as the message published by ServiceControl uses Json serialization.
 * Also customize the endpoint configuration such that the following conventions are used, as the `MessageFailed` event that is published by ServiceControl does not derive from `IEvent`.

NOTE: It's important that integration endpoints doesn't use the same `error` and `audit` queue as business endpoints since this might risk failures in the integration endpoint to cause an infinite feedback loop. Using the same `audit` queue will cause the integration messages to be included in search results in ServiceInsight. This will confuse users searching for given failure since both the failure and the failure notification will be shown to them. See also [recoverability](/nservicebus/recoverability/) and [audit queue settings](/nservicebus/operations/auditing.md).

WARNING: The ServiceControl event publishing mechanism is not compatible with Azure Service Bus Forwarding [topology](/transports/azure-service-bus/topologies.md) and RabbitMQ Direct and custom [topologies](/transports/rabbitmq/routing-topology.md). With these transports the endpoint subscribing to ServiceControl events has to use  the Endpoint Oriented and Conventional topologies respectively.

This code sample illustrates how to do this customization:

snippet: ServiceControlEventsConfig

 * The endpoint will also need a message handler, that handles the `MessageFailed` event. In the following example, there is also a simple HTTP call to HipChat's API to show how to possibly integrate with a 3rd party system to provide notification of the error.

snippet: MessageFailedHandler


### Common information contained in events

Both heartbeat and custom check events contain identifying information about the host and the endpoint.


### Alerting on HeartbeatStopped Event

[Heartbeats](/servicepulse/intro-endpoints-heartbeats.md#active-vs-inactive-endpoints) are used to track endpoints health.

Once an endpoint stops sending heartbeats to the ServiceControl queue then a `HeartbeatStopped` event will be published.

The message contains the time it was detected and the last heartbeat time.

Similarly to the code above it is possible to subscribe to the event, handle it, and perform custom actions.


### Alerting on HeartbeatRestored Event

Once an endpoint resumes sending heartbeats to the ServiceControl queue then a `HeartbeatRestored` event will be published.

The event contains the time the heartbeat was restored.

Similarly to the code above it is possible to subscribe to the event, handle it and provide custom actions.


### Alerting on CustomCheckFailed Event

[Custom checks](/servicepulse/intro-endpoints-custom-checks.md) are used to alert OPS of possible issues with third parties.

Once a custom check fails a `CustomCheckFailed` event will be published.

The message contains the time it was detected and the failure reason.

Similarly to the code above it is possible to subscribe to the event, handle it, and provide custom actions.


### Alerting on CustomCheckSucceeded Event

Once a custom check succeeds ServiceControl will publish a `CustomCheckSucceeded` event.

The message contains the time it was detected.

Similarly to the code above it is possible to subscribe to the event, handle it, and perform custom actions.


## Decommissioning alert subscribers

ServiceControl uses [Event Publishing](/nservicebus/messaging/publish-subscribe/) to expose information to subscribers. When using a [persistence based transport](/nservicebus/messaging/publish-subscribe/#mechanics-persistence-based-message-driven) an internal reference will be kept to each subscriber. If a subscriber, for an event, cannot be contacted then a [log entry](logging.md) will be written with the following error:

```
Failed dispatching external integration event
```

An event will also be published and displayed in ServicePulse dashboard with the following text:

```
'EVENTTYPE' failed to be published to other integration points. Reason for failure: REASON.
```

To avoid this situation it is important to properly decommission an endpoint that subscribes to ServiceControl events. To do this [disable auto-subscription](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#disabling-auto-subscription) and then [unsubscribe from all events](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#manually-subscribing-to-a-message).
