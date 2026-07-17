### Interval

Specifies the maximum delay between sending metrics report messages.

The metrics plugin buffers measurements and sends a metric report message as soon as the buffer is half full, without waiting for the interval to elapse. When an endpoint instance is idle the plugin will still send a report at this interval so that ServiceControl monitoring knows that the instance is still running. When the endpoint is under load, the time between metric messages will be much shorter as the buffer fills faster.

> [!NOTE]
> The size of this buffer is fixed and cannot be adjusted. The size chosen is compatible with the maximum message size limits of all supported transports.

The recommended value is between 10 and 60 seconds.

1. Smaller values result in ServiceControl monitoring view to be updated faster, especially on the 1-minute timescale
2. Larger values result in less frequent ServiceControl monitoring view updates only when an instance is not under heavy load. This isn't affecting the monitoring view with 10-minutes or larger timescales

It is recommended to use higher values for systems that can have many endpoint instances or use a transport that might be affected by rate limiting (often cloud-based).

Example:

If the system has 500 instances and the interval is set to 5 seconds, an idle system will send on average 6000 messages per minute (500 × 12), which is 100 per second.
