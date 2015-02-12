---
title: Custom notification and alerting using ServiceControl events
summary: How to build a custom notification by subscribing to ServiceControl events
tags:
- ServiceControl
- ServicePulse
- Notifications
redirects:
- servicepulse/custom-notification-and-alerting-using-servicecontol-events
---

## Custom notification and alerting using ServiceControl events

ServiceControl events allow to build notifications/integrations that will alert of something going wrong in your system.

ServiceControl's endpoint plugins collect information from monitored NServiceBus endpoints. For more information see [ServiceControl Endpoint Plugins](/servicecontrol/plugins.md).

### Alerting on FailedMessages Event

Once a message ends up in the error queue ServiceControl will publish a [MessageFailed](https://github.com/Particular/ServiceControl.Contracts/blob/master/src/ServiceControl.Contracts/MessageFailed.cs) event. As you can see the message contains enough context to help identify the cause of the error, the endpoint, the time, the stack trace and more. if you need more information you can call ServiceControl's HTTP API.

### Subscribing to ServiceControl Events 

ServiceControl publishes `MessageFailed` event when a message gets to the error queue, let's see how we can tap in by subscribing to these events and act on them (send an email, pager duty and so on)...

To subscribe to the `MessageFailed` event:
- Create an [NServiceBus endpoint](/nservicebus/the-nservicebus-host.md)
- Install the `ServiceControl.Contracts` [nuget package](https://www.nuget.org/packages/ServiceControl.Contracts/).
- Add the message mapping in the `UnicastBusConfig` section of the endpoint's app.config so that this endpoint will subscribe to the events from ServiceControl as shown:

<!-- import ServiceControlEventsXmlConfig -->

- Customize the endpoint configuration to use `JsonSerializer` as the message published by ServiceControl uses Json serialization
- Also customize the endpoint configuration such that the following conventions are used, as the `MessageFailed` event that is published by ServiceControl does not derive from `IEvent`. 
The code sample to do both customizations is as shown below:

<!-- import ServiceControlEventsConfig -->

- The endpoint will also need a message handler, that handles the `MessageFailed` event. In the following example, there is also a simple HTTP call to HipChat's API to show how you could integrate with a 3rd party system to provide notification of the error.

<!-- import MessageFailedHandler -->

### Common information contained in events

Both heartbeat and custom check events contain identifying information about the host and the endpoint.

### Alerting on HeartbeatStopped Event

Heartbeats are used to track endpoints health see [this intro for more information](/servicepulse/intro-endpoints-heartbeats.md#active-vs-inactive-endpoints)

Once an endpoint stops sending heartbeats to ServiceControl queue ServiceControl will publish a [HeartbeatStopped](https://github.com/Particular/ServiceControl.Contracts/blob/master/src/ServiceControl.Contracts/HeartbeatStopped.cs) event. 

The message contains the time it was detected and the last heartbeat time.

Similarly to the code above you can subscribe to the event, handle it and provide custom actions.

### Alerting on HeartbeatRestored Event

Once an endpoint resumes sending heartbeats to ServiceControl queue ServiceControl will publish a [HeartbeatRestored](https://github.com/Particular/ServiceControl.Contracts/blob/master/src/ServiceControl.Contracts/HeartbeatRestored.cs) event. 

The event contains the time the heartbeat was restored.

Similarly to the code above you can subscribe to the event, handle it and provide custom actions.

### Alerting on CustomCheckFailed Event

Custom checks are used to alert OPS of possible issues with third parties see [this intro for more information](/servicepulse/intro-endpoints-custom-checks.md)

Once a custom check fails ServiceControl will publish a [CustomCheckFailed](https://github.com/Particular/ServiceControl.Contracts/blob/master/src/ServiceControl.Contracts/CustomCheckFailed.cs) event. 

The message contains the time it was detected and the failure reason.

Similarly to the code above you can subscribe to the event, handle it and provide custom actions.

### Alerting on CustomCheckSucceeded Event

Once a custom check succeeds ServiceControl will publish a [CustomCheckSucceeded](https://github.com/Particular/ServiceControl.Contracts/blob/master/src/ServiceControl.Contracts/CustomCheckSucceeded.cs) event. 

The message contains the time it was detected.

Similarly to the code above you can subscribe to the event, handle it and provide custom actions.
