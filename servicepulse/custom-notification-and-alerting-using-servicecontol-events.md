---
title: Custom notification and alerting using ServiceControl's events
summary: How to build a custom notification by subscribing to ServiceControl's events
tags:
- ServiceControl
- ServicePulse
- Notifications
---

## Custom notification and alerting using ServiceControl events

ServiceControl events allow to build notifications/integrations that will alert of something going wrong in your system.

ServiceControl's endpoint plugins collect information from monitored NServiceBus endpoints. For more information see [ServiceControl Endpoint Plugins](/servicecontrol/plugins.md).

### Alerting on FailedMessages Event

Once a message ends up in the error queue ServiceControl will publish a [MessageFailed](https://github.com/Particular/ServiceControl.Contracts/blob/master/src/ServiceControl.Contracts/MessageFailed.cs) event. As you can see the message contains enough context to help identify the cause of the error, the endpoint, the time, the stack trace and more. if you need more information you can call ServiceControl's HTTP API.

### Subscribing to ServiceControl Events 

ServiceControl publishes `MessageFailed` event when a message gets to the error queue, let's see how we can tap in by subscribing to these events and act on them (send an email, pager duty and so on)...

Let's see how we can subscribe to a `MessageFailed` Event and push a notification into HipChat. All it takes is to have an endpoint that subscribes to `MessageFailed`, and a simple HTTP call to HipChat's API.


#### Required endpoint configuration ServiceControl messages in Conventions

NOTE: The endpoint will need to match ServiceControl serializer: JsonSerializer

NOTE: In order for the endpoint to handle ServiceControl events you need to register them in the endpoint's message Conventions

<!-- import ServiceControlEventsConfig -->

#### Custom action example

<!-- import MessageFailedHandler -->

### Common information contained in events

Both heartbeat events contain identifying information about the host and the endpoint.

### Alerting on HeartbeatStopped Event

Heartbeats are used to track endpoints health see [this into for more information](/servicepulse/intro-endpoints-heartbeats.md#active-vs-inactive-endpoints)

Once an endpoint stops sending heartbeats to ServiceControl queue ServiceControl will publish a [HeartbeatStopped](https://github.com/Particular/ServiceControl.Contracts/blob/master/src/ServiceControl.Contracts/HeartbeatStopped.cs) event. 

The message contains the time it was detected and the last heartbeat time.

Similarly to the code above you can subscribe to the event, handle it and provide custom actions.

### Alerting on HeartbeatRestored Event

Once an endpoint resumes sending heartbeats to ServiceControl queue ServiceControl will publish a [HeartbeatRestored](https://github.com/Particular/ServiceControl.Contracts/blob/master/src/ServiceControl.Contracts/HeartbeatRestored.cs) event. 

The event contains the time the heartbeat was restored.

Similarly to the code above you can subscribe to the event, handle it and provide custom actions.