---
title: Message routing
summary: How NServiceBus routes messages between the endpoints.
reviewed: 2016-08-17
component: Core
tags:
- routing
redirects:
- nservicebus/how-do-i-specify-to-which-destination-a-message-will-be-sent
- nservicebus/messaging/specify-message-destination
- nservicebus/messaging/message-owner
- nservicebus/msmq/sender-side-distribution
related:
- samples/pubsub
- nservicebus/messaging/publish-subscribe
- nservicebus/messaging/routing-extensibility
---


## Routing concepts

Routing subsystem is responsible for finding destinations for the messages. In most cases the sending code should not care about the destination of the message being sent:

snippet: BasicSend

Based on the type of the message the routing provides the destination address.

partial: concepts


## Command routing

As described in [messages, events and commands](/nservicebus/messaging/messages-events-commands.md), NServiceBus distinguishes several types of messages. Command messages are always routed to a single logical endpoint. 

partial: commands

The per-namespace routes override assembly-level routes and the per-type routes override both namespace and assembly routes. 


### Overriding the destination

In specific cases, like sending to self or to a particular queue (e.g. for integration purposes), the routing configuration can be overridden when sending a given message. Refer to documentation on [sending](/nservicebus/messaging/send-a-message.md) for further details.


## Event routing

Events can be received by multiple logical endpoints, however even in case of scale out each event will be received only by one physical instance of any logical subscriber. Before the message is published and delivered, the subscriber has to express its interest in a given type of event.


### Native

[Multicast transports](/transports/#types-of-transports-multicast-enabled-transports) support Publish-Subscribe pattern natively. In this case the subscriber uses the APIs of the transport to create a route for a given subscribed message type.

Note: Azure Service Bus `EndpointOrientedTopology` requires [publishers names](/transports/azure-service-bus/publisher-names-configuration.md) to be configured.


### Message-driven

[Other transports](/transports/#types-of-transports-unicast-only-transports) do not support Publish-Subscribe natively. These transports emulate the publish behavior by sending message to each subscriber directly. To do this, the publisher endpoint has to know its subscribers and subscribers have to notify the publisher about their interest in a given event type. The notification message (known as *subscribe* message) has to be routed to the publisher.

partial: events

In the `UnicastBusConfig/MessageEndpointMappings` configuration section publishers are registered in the same way as the command destinations are defined. If a given assembly or namespace contains both events and commands, the mapping will recognize that fact and configure the routing correctly (both commands and subscribe messages will be routed to the destination specified in `Endpoint` attribute).

`MessageEndpointMappings` routing configuration can take advantage of message inheritance: base types "inherit" routes from the derived types (opposite to member inheritance in .NET). If both `EventOne` and `EventTwo` inherit from `BaseEvent`, when subscribing to `BaseEvent` the subscription messages are sent to publishers of both `EventOne` and `EventTwo`.


## Reply routing

Replies are always routed based on the initial message `ReplyTo` header regardless of replier's routing configuration. Only the sender of the initial message can influence the routing of a reply. Refer to documentation on [sending](/nservicebus/messaging/send-a-message.md) for further details.
