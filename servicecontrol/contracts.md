---
title: Using ServiceControl Events
summary: Build custom notifications by subscribing to ServiceControl events
reviewed: 2024-10-14
component: ServiceControlContracts
isLearningPath: true
redirects:
 - servicepulse/custom-notification-and-alerting-using-servicecontol-events
 - servicepulse/custom-notification-and-alerting-using-servicecontrol-events
 - servicecontrol/external-integrations
related:
 - samples/servicecontrol/events-subscription
 - samples/servicecontrol/azure-monitor-events
 - monitoring/heartbeats/notification-events
 - monitoring/custom-checks/notification-events
---

ServiceControl events enable the construction of custom notifications and integration that will alert when something goes wrong in the system. In addition to [monitoring the error queue](/nservicebus/recoverability/configure-error-handling.md), ServiceControl receives information from the [NServiceBus.Heartbeat](/monitoring/heartbeats/) and [NServiceBus.CustomChecks](/monitoring/custom-checks/) packages. When messages fail, heartbeats fail to arrive, or custom checks are reported, events published by ServiceControl enable a subscribing endpoint to notify operations personnel however the developer wishes.

See [Monitor with ServiceControl events](/samples/servicecontrol/events-subscription/) for a sample.

> [!NOTE]
> External notification events are sent in batches. If a problem is encountered part way through a batch, the entire batch will be re-sent. This can result in receiving multiple events for a single notification.

### Alerting on FailedMessages events

Once a message ends up in the error queue, ServiceControl will publish a `MessageFailed` event. The message contains:

 * the endpoint that sent and received the message
 * the failure cause (i.e. the exception type and message)
 * the message headers
 * the message body (if it is non-binary, smaller than 85 KB and full-text body indexing is enabled)


### Subscribing to ServiceControl events

ServiceControl publishes a `MessageFailed` event when a message arrives at the error queue. It is possible to subscribe to these events and act on them (e.g. send an email, trigger a text message, etc.).

To subscribe to the `MessageFailed` event:

 * Create an [NServiceBus endpoint](/nservicebus/hosting/nservicebus-host/).
 * Install the [ServiceControl.Contracts NuGet package](https://www.nuget.org/packages/ServiceControl.Contracts/).
 * Customize the endpoint configuration to use `JsonSerializer` as the message published by ServiceControl uses JSON serialization.
 * Customize the endpoint configuration so that the following conventions are used, as the events published by ServiceControl do not derive from `IEvent`.

This code sample illustrates how to do this customization:

snippet: ServiceControlEventsConfig

 * The endpoint will also need a message handler that handles the `MessageFailed` event. In the following example, there is a simple HTTP call to HipChat's API to show how to integrate with a third-party system to provide notification of the error.

snippet: MessageFailedHandler

> [!WARNING]
> Endpoints that subscribe to ServiceControl events should _not_ use the same `error` and `audit` queues as other endpoints. Using the same `error` queue could cause an infinite feedback loop if processing a `MessageFailed` message failed. Using the same `audit` queue will cause the processing of the `MessageFailed` messages to be included in the ServicePulse messages search results. This could confuse users searching for a given failure since both the failure and the failure notification will be shown. See also: [Recoverability](/nservicebus/recoverability/) and [Audit Queue Settings](/nservicebus/operations/auditing.md).


#### Registering the publisher for message-driven publish/subscribe

Transports that use [message-driven publish-subscribe](/nservicebus/messaging/publish-subscribe/) must have the ServiceControl instance registered as the publisher of the `MessageFailed` event.

For NServiceBus version 6 and higher, the [routing config code API](/nservicebus/messaging/routing.md#event-routing-message-driven) can be used:

snippet: ServiceControlPublisherConfig

For NServiceBus version 5 and below, add the message mapping in the `UnicastBusConfig` section of the endpoint's app.config :

snippet: ServiceControlEventsXmlConfig

> [!NOTE]
> Transports that [natively support publish and subscribe](/transports/types.md#multicast-enabled-transports) do not require any additional configuration.


### Monitoring events

ServiceControl will also publish events based on collected monitoring data.

See [Heartbeat Notification Events](/monitoring/heartbeats/notification-events.md) and [Custom Check Notification Events](/monitoring/custom-checks/notification-events.md) for a description of these events.

## Other events

> [!NOTE]
> Events described in this section are published by ServiceControl starting with version 4.17.

ServiceControl will also publish events related to archiving and retrying messages:
- `FailedMessagesArchived`: Event emitted for failed messages that were archived, indicating they won’t be retried
- `FailedMessagesUnArchived`: Event emitted for failed messages that were un-archived (restored from the archive), making them eligible for retry or further action
- `MessageFailureResolvedByRetry`: Event emitted by ServiceControl for each failed message that succeeded after retrying
- `MessageFailureResolvedManually`: Event emitted by ServiceControl for each failed message that was manually marked as resolved, typically via the "Resolve" or "Resolve All" actions in ServicePulse
- `MessageEditedAndRetried`: Event emitted by ServiceControl every time that a failed message was [edited and retried](/servicepulse/intro-editing-messages.md)

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

## Disabling integration 

In systems that have significant load but not having subscribers for the integration events it can be beneficial to disable the intergration to prevent unneeded traffic to the broker for these events. The integration can be disabled by setting [`DisableExternalIntegrationsPublishing` to `True`](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontroldisableexternalintegrationspublishing).
