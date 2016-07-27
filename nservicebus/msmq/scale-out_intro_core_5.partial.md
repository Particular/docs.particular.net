
In NServiceBus Version 5 and below it is necessary to use the [distributor](/nservicebus/msmq/scalability-and-ha/distributor/) in order to scale out an endpoint using the MSMQ transport. That is caused by limitations imposed by MSMQ on remote receives.

The role of the distributor is to forward incoming messages to a number of workers in order to balance the load. The workers are "invisible" to the outside world because all the outgoing messages contain the distributor's (not the worker's) address in the `reply-to` header.

The main issue with distributor is the throughput limitation due to the fact that, for each message forwarded to worker, there were additional two messages exchanged between the worker and the distributor.


##  Individualizing queue names when scaling out

NOTE: This is relevant to NServiceBus Version 5.2 and above.

It can be beneficial to run with a unique input queue per endpoint instance when scaling out since this avoids being limited by the throughput of a single queue. In this case all instances should share the same storage (for sagas, timeouts etc). This is achieved by keeping endpoint name the same since NServiceBus by default uses the endpoint name to select the database.

For this reason a new API has been from NServiceBus Version 5.2 that allows queue name individualization per endpoint while still keeping the endpoint name stable.

snippet: UniqueQueuePerEndpointInstance

This will tell NServiceBus to use the suffix registered by the transport or host to make sure that each instance of the endpoint will be unique. 

In case of MSMQ transport the functionality is provided by the underlying transport infrastructure, since it is commonly scaled out by running multiple instances on different machines and that will give them a unique address out of the box. Eg: `sales@server1, sales@serverN` etc.

If full control over the suffix is require or if the transport hasn't registered a default then it can be controlled using:

snippet: UniqueQueuePerEndpointInstanceWithSuffix

NOTE: In NServiceBus Version 5 and below there is no built-in mechanism of distributing the load between endpoints with individualized queue names. To use this feature to overcome the throughput limitation of single queues by using this, the distribution needs to be managed manually.
