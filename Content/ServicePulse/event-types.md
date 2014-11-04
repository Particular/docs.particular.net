---
title: ServicePulse events
summary: 'Introduction to ServicePulse monitoring events'
tags: [ServicePulse, Concepts, Monitoring Events]
---

ServicePulse is the Particular platform monitoring front-end, powered by ServiceControl. ServicePulse gives you an overview of the system health, based on endpoints heartbeats and custom checks, and a detailed view of all the failed messages

You can consume the same information not only via the ServicePulse web interface but also subscribing to ServicePulse events broadcasted by the ServiceControl endpoint.

To subscribe to monitoring events is enough from a standard NServiceBus endpoint to add a reference to the `ServiceControl.Contracts` package, available via [NuGet](https://www.nuget.org/packages/ServiceControl.Contracts/), and configure the endpoint to subscribe to events published by the ServiceControl endpoint at the following address:
`Particular.ServiceControl`.

Actually there a 3 different monitoring events: HeartbeatStopped, HeartbeatRestored and MessageFailed.

### HeartbeatStopped

The `HeartbeatStopped` event is published each time the monitoring infrastructure does not receive the heartbeat, from an endpoint, within the expected amount of time.

### HeartbeatRestored

The `HeartbeatRestored` event is published to notify that a previously stopped heartbeat has been restored.

### MessageFailed

The `MessageFailed` event is published to notify that a message has failed all the First Level Retry steps and all the Second Level Retry steps and has reached the configured error queue. The event itself carries all the details of the failure and has a `MessageStatus` enumeration that details the type of failure:

* `Failed`: The message has failed and has arrived for the first time in the error queue;
* `RepeatedFailure`: The message has failed multiple times;
* `ArchivedFailure`: The message has been archived;