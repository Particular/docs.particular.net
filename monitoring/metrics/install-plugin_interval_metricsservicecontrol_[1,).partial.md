### Interval

Specifies the maximum delay between sending metrics report messages.

The metrics plugin has a buffer and when that overflows a metric report message is sent and the buffer is cleared. When an endpoint instance is idle it will send metrics report messages at this interval to indicate it is idle but when the endpoint is under load the interval between metric messages will be much shorter as this buffer fills faster.

The size of this buffer cannot be adjusted and uses a value that cannot maximum size limit errors with any of the supported transports.

The recommended value is between 10 and 60 seconds.

1. Smaller values result in ServiceControl monitoring view to be updated faster, especially on the 1-minute timescale
2. Larger values result in less frequent ServiceControl monitoring view updates only when an instance is not under heavy load. This isn't affecting the monitoring view with 10-minutes or larger timescales

It is recommended to use higher values for systems that can have many endpoint instances or use a transport that might be affected by rate limiting (often cloud-based).

Example:

If the system has 500 instances and the interval is set to 5 seconds an idle system will send on average 6.000 metric messages per minute (500 * 6) / 100 per second.
