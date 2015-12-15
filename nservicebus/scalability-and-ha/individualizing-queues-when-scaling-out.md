---
title: Individualizing queue names when Scaling-Out
summary: How to enable individualizing queue names when Scaling-Out endpoints
tags: 
- Scale Out
redirects:
 - nservicebus/individualizing-queues-when-scaling-out
---

INFO: This is relevant to versions 5.2 and above.

Depending on the transport you use it can be beneficial to run with a unique input queue per endpoint instance when scaling out since this avoids beeing limited by the throughput of a single queue. You'd still want to have all those instances share the same storage for eg. sagas, timeouts etc. and that is achieved by keeping endpoint name the same since NServiceBus by default uses the endpoint name to select the database.

For this reason we have introduced a new API from NServiceBus Version 5.2 that allows you to enable queue name individualization per endpoint while still keeping the endpoint name stable.

<!-- import UniqueQueuePerEndpointInstance --> 

This will tell NServiceBus to use the suffix registered by the transport or host to make sure that each instance of your endpoint will be unique. For MSMQ this is a no-op since you commonly scaled them out by running multiple instances on different machines and that will give them a unique address out of the box. Eg: `sales@server1, sales@serverN` etc.

For broker transports that's no longer true, hence the need for a suffix. For on-premise operations the machine name is likely to be used and in cloud scenarios like on Azure the role instance id is better suited since machines will dynamically change. 

RabbitMQ example:  `sales-server1, sales-serverN` 
Azure ServiceBus example:  `sales-1, sales-N` where N is the role instance id

If you need full control over the suffix or if the transport hasn't registered a default you can control it using:

<!-- import UniqueQueuePerEndpointInstanceWithSuffix --> 
