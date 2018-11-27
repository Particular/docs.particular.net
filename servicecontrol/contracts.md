---
title: Using ServiceControl Events
summary: Build custom notifications by subscribing to ServiceControl events
reviewed: 2018-06-21
component: ServiceControlContracts
tags:
 - Notifications
redirects:
 - servicepulse/custom-notification-and-alerting-using-servicecontol-events
 - servicepulse/custom-notification-and-alerting-using-servicecontrol-events
 - servicecontrol/external-integrations
related:
 - samples/servicecontrol/events-subscription
 - monitoring/heartbeats/notification-events
 - monitoring/custom-checks/notification-events
---


## Custom notification and alerting using ServiceControl events

ServiceControl events enable the construction of custom notifications and integration that will alert when something goes wrong in the system.

WARNING: External notification events are sent in batches. If a problem is encountered part way through a batch, the entire batch will be re-sent. This can result in receiving multiple events for a single notification.

[ServiceControl endpoint plugins](/servicecontrol/plugins/) collect information from monitored NServiceBus endpoints.


### Alerting on FailedMessages events

Once a message ends up in the error queue, ServiceControl will publish a `MessageFailed` event. The message contains:

 * The processing of the message (i.e. the endpoint that sent and received the message).
 * The failure cause (i.e. the exception type and message).
 * The message itself (i.e. the headers and, if using non-binary serialization, the body).


### Subscribing to ServiceControl events

ServiceControl publishes a `MessageFailed` event when a message gets to the error queue. It is possible to subscribe to these events and act on them (e.g. send an email, trigger a text message, etc.).

To subscribe to the `MessageFailed` event:

 * Create an [NServiceBus endpoint](/nservicebus/hosting/nservicebus-host/).
 * Install the [ServiceControl.Contracts NuGet package](https://www.nuget.org/packages/ServiceControl.Contracts/).
 * Customize the endpoint configuration to use `JsonSerializer` as the message published by ServiceControl uses JSON serialization.
 * Customize the endpoint configuration so that the following conventions are used, as the `MessageFailed` event that is published by ServiceControl does not derive from `IEvent`.

This code sample illustrates how to do this customization:

snippet: ServiceControlEventsConfig

 * The endpoint will also need a message handler that handles the `MessageFailed` event. In the following example, there is a simple HTTP call to HipChat's API to show how to integrate with a third-party system to provide notification of the error.

snippet: MessageFailedHandler

WARNING: Endpoints that subscribe to ServiceControl events should _not_ use the same `error` and `audit` queues as other endpoints. Using the same `error` queue could cause an infinite feedback loop if processing a `MessageFailed` message failed. Using the same `audit` queue will cause the processing of the `MessageFailed` messages to be included in the ServiceInsight search results. This could confuse users searching for a given failure since both the failure and the failure notification will be shown. See also: [Recoverability](/nservicebus/recoverability/) and [Audit Queue Settings](/nservicebus/operations/auditing.md).


#### Registering the publisher for message-driven publish/subscribe

Transports that use [message-driven publish-subscribe](/nservicebus/messaging/publish-subscribe/) must have the ServiceControl instance registered as the publisher of the `MessageFailed` event.

For NServiceBus version 6 and higher, the [routing config code API](/nservicebus/messaging/routing.md#event-routing-message-driven) can be used:

snippet: ServiceControlPublisherConfig

For NServiceBus version 5 and below, add the message mapping in the `UnicastBusConfig` section of the endpoint's app.config :

snippet: ServiceControlEventsXmlConfig

NOTE: Transports that [natively support publish and subscribe](/transports/types.md#multicast-enabled-transports) do not require any additional configuration.


### Monitoring events

ServiceControl will also publish events based on collected monitoring data.

See [Heartbeat Notification Events](/monitoring/heartbeats/notification-events.md) and [Custom Check Notification Events](/monitoring/custom-checks/notification-events.md) for a description of these events.


## Decommissioning subscribers to ServiceControl events

ServiceControl uses [event publishing](/nservicebus/messaging/publish-subscribe/) to expose information to subscribers. When using a [persistence-based transport](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based) an internal reference will be kept to each subscriber. If a subscriber for an event cannot be contacted then a [log entry](logging.md) will be written with the following error:

```
Failed dispatching external integration event
```

An event will also be published and displayed in the ServicePulse dashboard with the following text:

```
'EVENTTYPE' failed to be published to other integration points. Reason for failure: REASON.
```

To avoid this situation it is important to properly decommission an endpoint that subscribes to ServiceControl events. To do this, [disable auto-subscription](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#disabling-auto-subscription) and then [unsubscribe from all events](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#manually-subscribing-to-a-message).
