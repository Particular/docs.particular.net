---
title: Using ServiceControl events
summary: How to build a custom notification by subscribing to ServiceControl events
tags:
- ServiceControl
- ServicePulse
- Notifications
redirects:
- servicepulse/custom-notification-and-alerting-using-servicecontol-events
- servicepulse/custom-notification-and-alerting-using-servicecontrol-events
- servicecontrol/external-integrations
---


## Custom notification and alerting using ServiceControl events

ServiceControl events allow to build notifications/integrations that will alert of something going wrong in the system.

ServiceControl's endpoint plugins collect information from monitored NServiceBus endpoints. For more information see [ServiceControl Endpoint Plugins](/servicecontrol/plugins/).


### Alerting on FailedMessages Event

Once a message ends up in the error queue ServiceControl will publish a `MessageFailed` event. The message contains:

 * The processing of the message (e.g. which endpoint sent and received the message)
 * The failure cause (e.g. the exception type and message)
 * The message itself (e.g. the headers and, if using non-binary serialization, also the body)


### Subscribing to ServiceControl Events

ServiceControl publishes `MessageFailed` event when a message gets to the error queue, it is possible to subscribe to these events and act on them (send an email, pager duty and so on).

To subscribe to the `MessageFailed` event:
- Create an [NServiceBus endpoint](/nservicebus/hosting/nservicebus-host/)
- Install the [ServiceControl.Contracts NuGet package](https://www.nuget.org/packages/ServiceControl.Contracts/).
- Add the message mapping in the `UnicastBusConfig` section of the endpoint's app.config so that this endpoint will subscribe to the events from ServiceControl as shown:

snippet:ServiceControlEventsXmlConfig

- Customize the endpoint configuration to use `JsonSerializer` as the message published by ServiceControl uses Json serialization
- Also customize the endpoint configuration such that the following conventions are used, as the `MessageFailed` event that is published by ServiceControl does not derive from `IEvent`.

NOTE: It's important that integration endpoints doesn't use the same `error` and `audit` queue as business endpoints since this might risk failures in the integration endpoint to cause an infinite feedback loop. Using the same `audit` queue will cause the integration messages to be included in search results in ServiceInsight. This will confuse users saerching for given failure since both the failure and the failure notification will be shown to them. See also [adjust error queue settings](/nservicebus/errors/) and [audit queue settings](/nservicebus/operations/auditing.md).

The code sample to do both customizations is as shown below:

snippet:ServiceControlEventsConfig

- The endpoint will also need a message handler, that handles the `MessageFailed` event. In the following example, there is also a simple HTTP call to HipChat's API to show how to possibly integrate with a 3rd party system to provide notification of the error.

snippet:MessageFailedHandler


### Common information contained in events

Both heartbeat and custom check events contain identifying information about the host and the endpoint.


### Alerting on HeartbeatStopped Event

Heartbeats are used to track endpoints health see [this intro for more information](/servicepulse/intro-endpoints-heartbeats.md#active-vs-inactive-endpoints)

Once an endpoint stops sending heartbeats to ServiceControl queue ServiceControl will publish a `HeartbeatStopped` event.

The message contains the time it was detected and the last heartbeat time.

Similarly to the code above it is possible to subscribe to the event, handle it, and perform custom actions.


### Alerting on HeartbeatRestored Event

Once an endpoint resumes sending heartbeats to ServiceControl queue ServiceControl will publish a `HeartbeatRestored` event.

The event contains the time the heartbeat was restored.

Similarly to the code above it is possible to subscribe to the event, handle it and provide custom actions.


### Alerting on CustomCheckFailed Event

Custom checks are used to alert OPS of possible issues with third parties see [this intro for more information](/servicepulse/intro-endpoints-custom-checks.md)

Once a custom check fails ServiceControl will publish a `CustomCheckFailed` event.

The message contains the time it was detected and the failure reason.

Similarly to the code above it is possible to subscribe to the event, handle it and provide custom actions.


### Alerting on CustomCheckSucceeded Event

Once a custom check succeeds ServiceControl will publish a `CustomCheckSucceeded` event.

The message contains the time it was detected.

Similarly to the code above it is possible to subscribe to the event, handle it, and perform custom actions.


## Decommissioning alert subscribers

ServiceControl uses [Event Publishing](/nservicebus/messaging/publish-subscribe/) to send alerts to all subscribers. If using a [persistence based transport](/nservicebus/messaging/publish-subscribe/#mechanics-persistence-based) then ServiceControl will keep an internal reference to each subscriber. If a subscriber for an alert cannot be contacted then ServiceControl will shut itself down after a few attempts.

To prevent this situation it is important to properly decommission an endpoint that subscribes to ServiceControl events. To do this [disable auto-subscription](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#disabling-auto-subscription) and then [unsubscribe to all events](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#how-to-manually-subscribe-to-a-message).