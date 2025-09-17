---
title: Scaling Out With Sender-side Distribution
summary: How to scale out with sender-side distribution when using the MSMQ transport
component: MsmqTransport
reviewed: 2025-09-17
versions: '[6,)'
related:
 - transports/msmq/routing
 - nservicebus/messaging/routing
 - samples/scaleout/senderside
---

Endpoints using the MSMQ transport cannot use the competing consumers pattern to scale out by adding additional worker instances. Sender-side distribution is a method of scaling out an endpoint using the MSMQ transport without relying on a centralized distributor assigning messages to available workers.

When using sender-side distribution:

 * Multiple endpoint instances (deployed to different servers) can process a message that requires scaled-out processing.
 * A client sending a message knows all the endpoint instances available to process the message.
 * The client sends a message to a worker endpoint instance based on round-robin distribution or a custom distribution strategy.

Using sender-side distribution requires two parts. The first part maps message types to logical endpoints and occurs in code. The second part maps logical endpoints to physical endpoint instances running on a specific machine.

## Mapping logical endpoints

To map message types to logical endpoints, use the following configuration:

snippet: Routing-MapMessagesToLogicalEndpoints

This creates mappings specifying that the `AcceptOrder` command is handled by the **Sales** endpoint, while the **Shipping** endpoint handles the `SendOrder` command.

Meanwhile, the logical-to-physical mappings will be configured in the `instance-mapping.xml` file, as this information is an operational concern that must be changed for deployment to multiple machines.

> [!WARNING]
> If a message is mapped in an App.config file via the `UnicastBusConfig/MessageEndpointMappings` configuration section, that message cannot participate in sender-side distribution. The endpoint address specified by a message endpoint mapping is a physical address (`QueueName@MachineName`, where `MachineName` is assumed to be `localhost` if omitted) which combines the message-to-owner-endpoint and endpoint-to-physical-address concerns in a way that can't be separated.

## Mapping physical endpoint instances

The routing configuration file specifies how logical endpoint names are mapped to physical queues on specific machines:

snippet: InstanceMappingFile-ScaleOut

To read more about the instance mapping, refer to [MSMQ Routing](/transports/msmq/routing.md).

### Message distribution

Every message is delivered to a single physical instance of the logical endpoint. When scaling out, multiple instances of a single logical endpoint are registered in the routing system. Each outgoing message must undergo the distribution process to determine which instance will receive this particular message. By default, a round-robin algorithm is used to determine the destination. Routing extensions can override this behavior by registering a custom `DistributionStrategy` for a destination endpoint.

snippet: RoutingExtensibility-Distribution

snippet: RoutingExtensibility-DistributionStrategy

To learn more about creating custom distribution strategies, see the [fair distribution sample](/samples/routing/fair-distribution/).

## Events and subscriptions

**Subscription requests**: Each subscriber endpoint instance will send a subscription message to each configured publisher instance at startup. Each publisher instance receives a subscription request and stores it. In most cases, the subscription storage is shared.

**Events**: When an event is published, it will be sent to only one of the endpoint instances. Which instance depends on the [distribution strategy](#mapping-physical-endpoint-instances-message-distribution)

## Limitations

Sender-side distribution does not use message processing confirmations. Therefore, the sender has no feedback on the availability of workers and, by default, sends the messages in a round-robin behavior. Should one of the nodes stop processing, the messages will pile up in its input queue. As such, nodes running in sender-side distribution mode require more careful monitoring.

include: sender-side-distribution-with-distributor

## Decommissioning endpoint instances

For the reasons outlined above, when scaling down (removing a "target" endpoint instance from service), it is essential to decommission the instance properly:

 1. Change the instance mapping file to remove the target endpoint instance.
 1. Ensure the updated instance mapping information is distributed to all endpoint instances that might send a message to the target endpoint.
 1. Allow time (30 seconds by default) for all endpoints to reread the instance mapping file and ensure no new messages arrive in the target instance's queue.
 1. Allow the target endpoint instance to complete processing all messages in its queue.
 1. Disable the target endpoint instance.
 1. Check the input queue of the decommissioned instance for leftover messages and move them to other instances if necessary.
