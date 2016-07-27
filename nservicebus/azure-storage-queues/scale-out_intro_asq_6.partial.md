
Azure Storage Queues transport scales out by adding competing consumers receiving from a single queue. An additional feature of Azure transports is ability to [use individualized queues](individualizing-queues-when-scaling-out.md) (based on the *role instance ID*).

This feature is, however, *not intended* for scaling out as there is no built-in distribution mechanism between that would route messages to the individualized queues.


##  Individualizing queue names when scaling out

NOTE: This is relevant to NSerivceBus Versions 5.2 and above.

It can be beneficial to run with a unique input queue per endpoint instance when scaling out since this avoids being limited by the throughput of a single queue. In this case all instances should share the same storage (for sagas, timeouts etc). This is achieved by keeping endpoint name the same since NServiceBus by default uses the endpoint name to select the database.

For this reason a new API has been from NServiceBus Version 5.2 that allows queue name individualization per endpoint while still keeping the endpoint name stable.

snippet: UniqueQueuePerEndpointInstance

This will tell NServiceBus to use the suffix registered by the transport or host to make sure that each instance of the endpoint will be unique. 

Broker transports, such as Azure Storage Queues transport, need to specify a suffix per endpoint instance. For on-premise operations the machine name is likely to be used and in cloud scenarios like on Azure the role instance ID is better suited since machines will dynamically change.

The suffix can be also fully controlled using the configuration API:

snippet: UniqueQueuePerEndpointInstanceWithSuffix

NOTE: In NServiceBus Version 5 and below there is no built-in mechanism of distributing the load between endpoints with individualized queue names. To use this feature to overcome the throughput limitation of single queues by using this, the distribution needs to be managed manually.