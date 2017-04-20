## Critical Time

**Counter Name:** `Critical Time`

**Added in:** Version 3

Critical Time is the time from a message being sent until successfully processed. This metric is useful for monitoring message Service-level agreement. For example "All orders should be processed within X seconds/minutes/hours". Define an SLA for each endpoint and use the `CriticalTime` counter to ensure it is adhered to.

**Versions 6 and above:** The value recorded in the TimeSent header is the time when the call to send the message is executed, not the actual time when the message was dispatched to the transport infrastructure. Since the outgoing messages in handlers are sent as a [batched](/nservicebus/messaging/batched-dispatch.md) operation, depending on how long the message handler takes to complete, the actual dispatch may happen later than the time recorded in the TimeSent header. For operations outside of handlers the recorded sent time is accurate.


### Configuration

This counter can be enabled using the the following code:

snippet: enable-criticaltime

In the NServiceBus Host this counter is enabled by default.


## SLA violation countdown

**Counter Name:** `SLA violation countdown`

**Added in:** Version 3

Acts as a early warning system to inform on the number of seconds left until the SLA for the endpoint is breached. This gives a system-wide counter that can be monitored without putting the SLA into the monitoring software. Just set that alarm to trigger when the counter goes below X, which is the time that the operations team needs to be able to take actions to prevent the SLA from being breached.


### Configuration

This counter can be enabled using the the following code:

snippet: enable-sla

See also [Performance Counters in the NServiceBus Host](/nservicebus/hosting/nservicebus-host/#performance-counters).


## Successful Message Processing Rate

**Counter Name:** `# of msgs successfully processed / sec`

**Added in:** Version 4

The current number of messages processed successfully by the transport per second.


### Configuration

Enabled by default and will only write to the counter if it exists.


## Queue Message Receive Rate

**Counter Name:** `# of msgs pulled from the input queue /sec`

**Added in:** Version 4

The current number of messages pulled from the input queue by the transport per second.


### Configuration

Enabled by default and will only write to the counter if it exists.


## Failed Message Processing Rate

**Counter Name:** `# of msgs failures / sec`

**Added in:** Version 4

The current number of failed processed messages by the transport per second.


### Configuration

Enabled by default and will only write to the counter if it exists.