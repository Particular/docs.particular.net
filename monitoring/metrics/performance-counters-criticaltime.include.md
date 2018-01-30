## Critical Time

**Counter Name:** `Critical Time`

**Added in:** Version 3

Critical Time is the time from a message being sent until successfully processed. This metric is useful for monitoring a message's service-level agreement. For example: "All orders should be processed within 30 seconds". Define an SLA for each endpoint and use the `CriticalTime` counter to ensure it is adhered to.


### Configuration

This counter can be enabled using the following code:

snippet: enable-criticaltime

In the NServiceBus host this counter is enabled by default.
