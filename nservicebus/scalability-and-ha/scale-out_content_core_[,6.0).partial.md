## MSMQ

Because of limitations of MSMQ related to remote receive, in order to scale out an MSMQ Version 5 (and below) endpoint have to use the [distributor](/nservicebus/scalability-and-ha/distributor/). The role of the distributor is to forward incoming messages to a number of workers in order to balance the load. The workers are "invisible" to the outside world because all the outgoing messages contain the distributor's (not the worker's) address in the `reply-to` header.

The main issue with distributor is the throughput limitation due to the fact that, for each message forwarded to worker, there were additional two messages exchanged between the worker and the distributor.


## SQL Server and RabbitMQ

Both SQL Server and RabbitMQ transports scale out by adding more receivers to the same queue, taking advantage of the competing consumers capability built into these transports. All the instances feeding off the queue have same *endpoint name* and *address* so they appear to the outside world as a single instance.

The benefit of this approach is zero configuration. New instances can be added by `xcopy`-ing the deployment folder. The potential downside is that total throughput of the endpoint is capped to the maximum throughput of a single queue in the underlying infrastructure.


## Azure Storage Queues and Azure Service Bus

Both Azure transports behave similarly to other broker transports (SQL Server and RabbitMQ): they scale out by adding competing consumers receiving from a single queue.