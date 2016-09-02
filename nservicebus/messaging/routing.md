---
title: Message routing
summary: How NServiceBus routes messages between the endpoints.
reviewed: 2016-08-17
component: Core
tags:
- routing
- message
- routing
- send
- publish
- reply
- Message Mapping
- Message destination
redirects:
- nservicebus/how-do-i-specify-to-which-destination-a-message-will-be-sent
- nservicebus/messaging/specify-message-destination
- nservicebus/messaging/message-owner
related:
- samples/pubsub
- nservicebus/messaging/publish-subscribe
- nservicebus/messaging/routing-extensibility
- nservicebus/msmq/scalability-and-ha/sender-side-distribution
---


## Routing concepts

Routing subsystem is responsible for finding destinations for the messages. In most cases the sending code should not care about the destination of the message being sent:

snippet:BasicSend

Based on the type of the message the routing provides the destination address.

partial:concepts


## Command routing

As described [here](/nservicebus/messaging/messages-events-commands.md), NServiceBus distinguishes several types of messages. Command messages are always routed to a single logical endpoint. 

partial:commands

Each entry in the collection has to specify the assembly where the messages are defined. In addition to that, a type name or the namespace name can be also specified for additional filtering. 

NOTE: For backwards compatibility a `Messages` attribute can be used instead of `Type` and `Namespace` attributes. 

snippet:endpoint-mapping-appconfig

The per-namespace routes override assembly-level routes and the per-type routes override both namespace and assembly routes.


### Overriding the destination

In specific cases, like sending to self or to a particular queue (e.g. for integration purposes), the routing configuration can be overridden when sending a given message. Refer to documentation on [sending](/nservicebus/messaging/send-a-message.md) for further details.


## Event routing

Events can be received by multiple logical endpoints, however even in case of scale-out each event will be received only by one physical instance of any logical subscriber. Before the message is published and delivered, the subscriber has to express its interest in a given type of event. 


### Native

Some transports support Publish-Subscribe pattern natively. In this case the subscriber uses the APIs of the transport to create a route for a given subscribed message type.


### Emulated

Other transports do not support Publish-Subscribe natively. These transports emulate the publish behavior by sending message to each subscriber directly. To do this, the publisher endpoint has to know it's subscribers and subscribers have to notify the publisher about their interest in a given event type. The notification message (known as *subscribe* message) has to be routed to the publisher.

partial:events

In the `UnicastBusConfig/MessageEndpointMappings` configuration section publishers are registered in the same way as the command destinations are defined. If a given assembly or namespace contains both events and commands, the mapping will recognize that fact and configure the routing correctly (both commands and subscribe messages will be routed to the destination specified in `Endpoint` attribute).


## Reply routing

Replies are always routed based on the initial message `ReplyTo` header regardless of replier's routing configuration. Only the sender of the initial message can influence the routing of a reply. Refer to documentation on [sending](/nservicebus/messaging/send-a-message.md) for further details.
