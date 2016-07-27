
NServiceBus Version 6 and above has a unified scalability model which is based on the concept of endpoint instance ID. Each deployment of NServiceBus can (but does not have to) be assigned an *instance ID*.

When instance ID is assigned, NServiceBus spins up an additional receiver for the queue which name is based on that ID, e.g. if the endpoint name is `Sales` and instance ID is `Green` that instance will try to receive from queues `Sales` and `Sales-Green` (actual queue names depend on the underlying transport).

It is up to the sender to choose if it is going to treat the endpoint as a whole (and send its messages to `Sales` queue) or address individual instances (e.g. `Sales-Red`, `Sales-Green`, `Sales-Blue`).

In the first case the the scaling out happens by means of competing consumers. In the second case it is realized by sender using a round-robin algorithm to balance the load on receiver instances.

Because MSMQ does not allow performant remote receives in most cases scaling out requires sender-side round-robin distribution. When using MSMQ different instances are usually deployed to different (virtual) machines. The following routing file (see [file-based routing](/nservicebus/messaging/file-based-routing.md)) shows scaling out of the `Sales` endpoint. System administrators are able to spin-up new instances of the endpoint should the load increase and the only requirement is adding an entry to the routing file. No changes in the source code are required.

snippet:Routing-FileBased-MSMQ

The corresponding logical routing is

snippet:Routing-StaticRoutes-Endpoint-Msmq


WARNING: When using this scale out technique in a mixed version environment make sure to deploy a distributor in front of the scaled out version 6 endpoint if that endpoint needs to subscribe to events published by endpoints using versions lower than 6 (refer to [the distributor sample](/samples/scaleout/distributor/) for details). Otherwise each event will be delivered to every instance of the scaled out endpoint.


##  Individualizing queue names when scaling out

The concept of unique queue suffix is extended and becomes the endpoint instance ID:

snippet:UniqueQueuePerEndpointInstanceDiscriminator

This is consistent across all the transports, allowing round-robin sender-side distribution of messages between scaled-out endpoint instances.
