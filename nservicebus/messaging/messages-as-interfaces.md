---
title: Interfaces as messages
summary: Define interfaces as messages in NServiceBus to support multiple inheritance scenarios
component: core
reviewed: 2024-04-04
related:
- nservicebus/messaging/dynamic-dispatch-and-routing
redirects:
- nservicebus/messages-as-interfaces
- nservicebus/messaging/interfaces-as-messages
---

Events can be created on the fly from interfaces without first defining an explicit class implementing the interfaces. This technique can be used to support multiple inheritance for [polymorphic routing scenarios](./dynamic-dispatch-and-routing.md). In general, it is recommended to use [dedicated, simple types](/nservicebus/messaging/messages-events-commands.md) as messages instead.

## Sending interface messages

Interface messages can be sent using the following syntax:

snippet: InterfaceSend

Replies are supported via:

snippet: InterfaceReply

## Publishing interface messages

Interface messages can be published using the following syntax:

snippet: InterfacePublish

## Creating interface messages with IMessageCreator

If an interface message is needed before calling `Send` or `Publish`, use `IMessageCreator` directly to create the message instance:

snippet: IMessageCreatorUsage
