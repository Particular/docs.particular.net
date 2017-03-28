---
title: Scaling out with sender-side distribution
summary: How to scale out with sender-side distribution when using the MSMQ transport.
component: MsmqTransport
reviewed: 2016-11-07
versions: "[6,)"
tags:
 - scalability
 - Routing
 - MSMQ
redirects:
 - nservicebus/messaging/file-based-routing
related:
 - nservicebus/msmq/routing
 - nservicebus/messaging/routing
---

Endpoints using the MSMQ transport are unable to use the competing consumers pattern to scale out by adding additional worker instances. Sender-side distribution is a method of scaling out an endpoint using the MSMQ transport, without relying on a centralized [distributor](/nservicebus/msmq/distributor/) assigning messages to available workers.

When using sender-side distribution:

 * Multiple endpoint instances (deployed to different servers) are capable of processing a message that requires scaled-out processing.
 * A client sending a message is aware of all the endpoint instances that can process the message.
 * The client sends the message to a worker endpoint instance based on round-robin distribution, or a custom distribution strategy.

Using sender-side distribution requires two parts. The first part maps message types to logical endpoints, and occurs in code. The second part maps logical endpoints to physical endpoint instances running on a specific machine.


## Mapping logical endpoints

To map message types to logical endpoints, use the following config:

snippet: Routing-MapMessagesToLogicalEndpoints

This creates mappings specifying that the `AcceptOrder` command is handled by the **Sales** endpoint, while the `SendOrder` command is handled by the **Shipping** endpoint.

Meanwhile, the logical-to-physical mappings will be configured in the `instance-mapping.xml` file, as this information is an operational concern that must be changed for deployment to multiple machines.

WARNING: If a message is mapped in an App.config file via the `UnicastBusConfig/MessageEndpointMappings` configuration section, then that message cannot participate in sender-side distribution. The endpoint address specified by a message endpoint mapping is a physical address (`QueueName@MachineName`, where machine name is assumed to be `localhost` if omitted) which combines the message-to-owner-endpoint and endpoint-to-physical-address concerns in a way that can't be separated.


## Mapping physical endpoint instances

The routing configuration file specifies how logical endpoint names are mapped to physical queues on specific machines:

snippet: InstanceMappingFile-ScaleOut

To read more about the instance mapping, refer to the [MSMQ routing page](/nservicebus/msmq/routing.md).


## Limitations

Sender-side distribution does not use message processing confirmations (the Distributor approach). Therefore the sender has no feedback on the availability of workers and, by default, sends the messages in a round-robin behavior. Should one of the nodes stop processing, the messages will start piling up in its input queue. As such nodes running in sender-side distribution mode require more careful monitoring compared to distributor workers.

include: sender-side-distribution-with-distributor

## Decommissioning endpoint instances

For the reasons outlined above, when scaling down (removing a "target" endpoint instance from service), it is important to properly decommission the instance:

 1. Change the instance mapping file to remove the target endpoint instance.
 1. Ensure that the updated instance mapping information is distributed to all endpoint instances that might send a message to the target endpoint.
 1. Allow time (30 seconds by default) for all endpoints to reread the instance mapping file, and ensure no new messages are arriving in the target instance's queue.
 1. Allow the target endpoint instance to complete processing all messages in its queue.
 1. Disable the target endpoint instance.
 1. Check the input queue of the decommissioned instance for leftover messages and move them to other instances if necessary.
