NServiceBus comes with a unified scalability model which is based on the concept of endpoint instance ID. Each deployment of NServiceBus can (but does not have to) be assigned an *instance ID*.

When instance ID is assigned, NServiceBus spins up an additional receiver for the queue which name is based on that ID, e.g. if the endpoint name is `Sales` and instance ID is `Green` that instance will try to receive from queues `Sales` and `Sales-Green` (actual queue names depend on the underlying transport).

It is up to the sender to choose if it is going to treat the endpoint as a whole (and send its messages to `Sales` queue) or address individual instances (e.g. `Sales-Red`, `Sales-Green`, `Sales-Blue`).

In the first case the the scaling out happens by means of competing consumers. In the second case it is realized by sender using a round-robin algorithm to balance the load on receiver instances. Both scale out approaches are supported by all transports, but some transports are better suited for one or the other.


## MSMQ

Because MSMQ does not allow performant remote receives in most cases scaling out requires sender-side round-robin distribution. 

Refer to the [Scaling out with sender-side distribution](/nservicebus/msmq/scalability-and-ha/sender-side-distribution.md) article for more details.

WARNING: When using sender-side distribution in a mixed version environment make sure to deploy a distributor in front of the scaled out version 6 endpoint if that endpoint needs to subscribe to events published by endpoints using versions lower than 6 (refer to [the distributor sample](/samples/scaleout/distributor/) for details). Otherwise each event will be delivered to every instance of the scaled out endpoint.


## Broker transports

The main difference when using broker transports is that queues are not attached to machines but rather are maintained by a central server (or cluster of servers). Such design enables usage of the competing consumers pattern for scaling out processing power. All instances have empty IDs and connect to the same infrastructure queue. When running on a broker transport such as RabbitMQ or SQL Server, it is enough to specify the logical routing:

snippet:Routing-StaticRoutes-Endpoint-Broker

New instances can be deployed by `xcopy`-ing the binaries to another machine or directory.

When there is a need to go past the throughput of a single infrastructure queue or to address each instance separately, instance IDs can be specified for each deployment of the endpoint. In this case, in addition to the shared `Sales` queue, there will be two instance-specific queues used by the `Sales` endpoint.

Some upstream endpoints might decide to still treat `Sales` as a single *thing* and depend on the logical routing only. These endpoints will continue to send their messages to the `Sales` queue. Others might use instance mapping files.

In that case the sender will use round-robin distribution when sending commands (exactly like in case of MSMQ). It will, however, publish events to the shared queue (`Sales`).