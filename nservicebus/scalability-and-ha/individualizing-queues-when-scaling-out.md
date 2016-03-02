---
title: Individualizing queue names when Scaling-Out
summary: Enabling individualizing queue names when Scaling-Out endpoints
tags:
- Scale Out
redirects:
 - nservicebus/individualizing-queues-when-scaling-out
---

## Version 6

The concept of unique queue suffix is extended and becomes the endpoint instance ID:

snippet:UniqueQueuePerEndpointInstanceDiscriminator

This is consistent across all the transports, allowing round-robin sender-side distribution of messages between scaled-out endpoint instances.


## Version 5

INFO: This is relevant to versions 5.2 and above.

Depending on the transport being used it can be beneficial to run with a unique input queue per endpoint instance when scaling out since this avoids being limited by the throughput of a single queue. In this case all instances should share the same storage (for sagas, timeouts etc). This is achieved by keeping endpoint name the same since NServiceBus by default uses the endpoint name to select the database.

For this reason a new API has been from NServiceBus Version 5.2 that allows queue name individualization per endpoint while still keeping the endpoint name stable.

snippet: UniqueQueuePerEndpointInstance

This will tell NServiceBus to use the suffix registered by the transport or host to make sure that each instance of the endpoint will be unique. For MSMQ this is a no-op since it is commonly scaled out by running multiple instances on different machines and that will give them a unique address out of the box. Eg: `sales@server1, sales@serverN` etc.

For broker transports that's no longer true, hence the need for a suffix. For on-premise operations the machine name is likely to be used and in cloud scenarios like on Azure the role instance id is better suited since machines will dynamically change.

 * RabbitMQ example: `sales-server1, sales-serverN`
 * Azure ServiceBus example: `sales-1, sales-N` where N is the role instance id
 * SQL Server transport ignores this setting altogether

If full control over the suffix is require or if the transport hasn't registered a default then it can be controlled using:

snippet: UniqueQueuePerEndpointInstanceWithSuffix

NOTE: In Version 5 there is no built-in mechanism of distributing the load between endpoints with individualized queue names. To use this feature to overcome the throughput limitation of single queues by using this, the distribution needs to be managed manually.