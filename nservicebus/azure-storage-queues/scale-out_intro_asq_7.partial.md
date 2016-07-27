
Version 6 of NServiceBus comes with a unified scalability model which is based on the concept of endpoint instance ID. Each deployment of NServiceBus can (but does not have to) be assigned an *instance ID*.

When instance ID is assigned, NServiceBus spins up an additional receiver for the queue which name is based on that ID, e.g. if the endpoint name is `Sales` and instance ID is `Green` that instance will try to receive from queues `Sales` and `Sales-Green` (actual queue names depend on the underlying transport).

It is up to the sender to choose if it is going to treat the endpoint as a whole (and send its messages to `Sales` queue) or address individual instances (e.g. `Sales-Red`, `Sales-Green`, `Sales-Blue`).

In the first case the the scaling out happens by means of competing consumers. In the second case it is realized by sender using a round-robin algorithm to balance the load on receiver instances.


##  Individualizing queue names when scaling out

The concept of unique queue suffix is extended and becomes the endpoint instance ID:

snippet:UniqueQueuePerEndpointInstanceDiscriminator

This is consistent across all the transports, allowing round-robin sender-side distribution of messages between scaled-out endpoint instances.