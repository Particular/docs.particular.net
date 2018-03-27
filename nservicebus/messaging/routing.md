---
title: Message routing
summary: How NServiceBus routes messages between endpoints.
reviewed: 2017-03-25
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

The routing subsystem is responsible for finding destinations for messages. In most cases, the code which sends messages should not specify the destination of the message being sent:

snippet: BasicSend

Based on the type of the message, the routing subsystem will provide the destination address.

partial: concepts


## Command routing

As described in [messages, events and commands](/nservicebus/messaging/messages-events-commands.md), NServiceBus distinguishes between these kinds of messages. Command messages are always routed to a single logical endpoint. 

partial: commands

Per-namespace routes override assembly-level routes while per-type routes override both namespace and assembly routes. 


### Overriding the destination

In specific cases, like sending to the same endpoint or to a specific queue (e.g. for integration purposes), the routing configuration can be overridden when sending a given message. Refer to documentation on [sending messages](/nservicebus/messaging/send-a-message.md) for further details.


## Event routing

When events are published, they can be received by multiple logical endpoints however, even in cases where those logical endpoints are scaled out, each event will be received only by one physical instance of a specific logical subscriber. It is important to note that before the event is published and delivered, the subscriber has to express its interest in that event by having a message handler for it.


### Native

[Multicast transports](/transports/#types-of-transports-multicast-enabled-transports) support the Publish-Subscribe pattern natively. In this case the subscriber uses the APIs of the transport to create a route for a given subscribed message type.

Note: The Azure Service Bus `EndpointOrientedTopology` requires [publishers names](/transports/azure-service-bus/publisher-names-configuration.md) to be configured.


### Message-driven

[Other transports](/transports/#types-of-transports-unicast-only-transports) do not support Publish-Subscribe natively. These transports emulate the publish behavior by sending message to each subscriber directly. To do this, the publisher endpoint has to know its subscribers and subscribers have to notify the publisher about their interest in a given event type. The notification message (known as *subscribe* message) has to be routed to the publisher.

partial: events

In the `UnicastBusConfig/MessageEndpointMappings` configuration section, publishers are registered in the same way as the command destinations are defined. If a given assembly or namespace contains both events and commands, the mapping will recognize that fact and configure the routing correctly (both commands and subscribe messages will be routed to the destination specified in the `Endpoint` attribute).

`MessageEndpointMappings` routing configuration can also take advantage of message inheritance: base types "inherit" routes from the derived types (opposite to member inheritance in .NET). If both `EventOne` and `EventTwo` inherit from `BaseEvent`, when subscribing to `BaseEvent` the subscription messages are sent to publishers of both `EventOne` and `EventTwo`.


## Reply routing

Reply message are always routed based on the `ReplyTo` header of the initial message regardless of the endpoint's routing configuration. Only the sender of the initial message can influence the routing of a reply. Refer to documentation on [sending messages](/nservicebus/messaging/send-a-message.md) for further details.
