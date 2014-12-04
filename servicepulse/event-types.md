---
title: ServicePulse events
summary: 'Introduction to ServicePulse monitoring events'
tags: [ServicePulse, Concepts, Monitoring Events]
---

ServicePulse is the Particular platform monitoring front-end, powered by ServiceControl. ServicePulse gives you an overview of the system health, based on endpoints heartbeats and custom checks, and a detailed view of all the failed messages.

Note: You can consume the same information not only via the ServicePulse web interface but also subscribing to ServicePulse events broadcasted by the ServiceControl endpoint.

### Heartbeats

#### HeartbeatStopped

The `HeartbeatStopped` event is published each time the monitoring infrastructure does not receive the heartbeat, from an endpoint, within the expected amount of time.

#### HeartbeatRestored

The `HeartbeatRestored` event is published to notify that a previously stopped heartbeat has been restored and the related endpoint is running as expected.

More details on [Endpoints and Heartbeats in ServicePulse](http://docs.particular.net/servicepulse/intro-endpoints-heartbeats).

### MessageFailed

The `MessageFailed` event is published to notify that a message has failed all the First Level Retry steps and all the Second Level Retry steps and has reached the configured error queue. The event itself carries all the details of the failure and has a `MessageStatus` enumeration that details the type of failure:

* `Failed`: The message has failed and has arrived for the first time in the error queue;
* `RepeatedFailure`: The message has failed multiple times;
* `ArchivedFailure`: The message has been archived;

More details on [Failed Message Monitoring in ServicePulse](http://docs.particular.net/servicepulse/intro-failed-messages).

### Custom Check / Periodic Check

Custom checks allows an endpoint to notify ServicePulse if a condition, that is business related, is not met. The endpoint heartbeat signals that the endpoint is running, a custom check can add more information that are business related, such as the endpoint is running and can access the external resources required to operate correctly.

More details on [Custom Check Monitoring in ServicePulse](http://docs.particular.net/servicepulse/intro-endpoints-custom-checks). 