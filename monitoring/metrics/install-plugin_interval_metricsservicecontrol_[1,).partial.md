### Interval

Specifies the maximum delay between sending metrics report messages.

The metrics plugin has a buffer and when that overflows a metric report message is send and the buffer is cleared. When an endpoint instance is idle it will send metrics report messages at this interval to indicate it is idle but when the endpoint is under load the interval between metric messages will be much shorter as this buffer fills faster.

The size of this buffer cannot be adjusted and uses a value that cannot maximum size limit errors with any of the supported transports.

The recommended value is between 10 and 60 seconds.

1. Shorter values result in ServiceControl monitoring to more quickly update especially on the 1 minute timescale but resultsin more metrics messages to be generated
2. Longer values result in less frequent updates when an instance is not under heavy load but this isn't really affecting the monitoring view with larger timespand like the 10 minutes or more timescales

It is recommend to use higher values for systems that can have many instances running or use transports that are might be affected by rate limiting (often cloud based).
