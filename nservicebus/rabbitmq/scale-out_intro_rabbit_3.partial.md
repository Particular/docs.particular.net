
RabbitMQ transport Version 3 and below scales out by adding more receivers to the same queue, taking advantage of the competing consumers capability built into the transport. All the instances feeding off the queue have the same *endpoint name* and *address* so they appear to the outside world as a single instance.

The benefit of this approach is zero configuration. New instances can be added by `xcopy`-ing the deployment folder. The potential downside is that total throughput of the endpoint is capped to the maximum throughput of a single queue in the underlying infrastructure.
