---
title: Monitoring NServiceBus Endpoints
summary: Monitoring in NServiceBus is easier than in regular three-tier systems due to queuing and message-based communication.
tags:
- Performance Counters
redirects:
- nservicebus/monitoring-nservicebus-endpoints
related:
- servicecontrol
- servicepulse
- nservicebus/errors
- nservicebus/operations/auditing
---

When a system is broken down into multiple processes, each with its own queue, you can identify which process is the bottleneck by examining how many messages (on average) are in each queue. The only issue is that without knowing the rate of messages coming into each queue, and the rate at which messages are being processed from each queue, you can't know how long messages are waiting in each queue, which is the primary indicator of a bottleneck.

Despite the many performance counters Microsoft provides for MSMQ (including messages in queues, machine-wide incoming and outgoing messages per second, and the total messages in all queues), there is no built-in performance counter for the time it takes a message to get through each queue.


## NServiceBus performance counters

NServiceBus includes several performance counters. They are installed under the `NServiceBus` category.

Since all performance counters in Windows are exposed via Windows Management Instrumentation (WMI), it is very straightforward to pull this information into your existing monitoring infrastructure.


### Critical Time

**Counter Name:** `Critical Time`

Monitors the age of the oldest message in the queue. This takes into account the whole chain, from the message being sent from the client machine until successfully processed by the server. Define an SLA for each of your endpoints and use the `CriticalTime` counter to make sure you adhere to it.


### SLA violation countdown

**Counter Name:** `SLA violation countdown`

Acts as a early warning system to tell you the number of seconds left until the SLA for the particular endpoint is breached. This gives you a system-wide counter that can be monitored without putting the SLA into your monitoring software. Just set that alarm to trigger when the counter goes below X, which is the time that your operations team needs to be able to take actions to prevent the SLA from being breached. To define the endpoint SLA, add the `[EndpointSLA]` attribute on your endpoint configuration. If self-hosting, use the `Configure.SetEndpointSLA()` method on the Fluent API instead. All processes running with the NServiceBus collect this information and the counters are enabled by default.


### Successful Message Processing Rate

**Counter Name:** `# of msgs successfully processed / sec`

The current number of messages processed successfully by the transport per second.


### Queue Message Receive Rate

**Counter Name:** `# of msgs pulled from the input queue /sec`

The current number of messages pulled from the input queue by the transport per second.


### Failed Message Processing Rate

**Counter Name:** `# of msgs failures / sec`

The current number of failed processed messages by the transport per second. 