---
title: Interfaces as messages
summary: Define messages as interfaces in NServiceBus to support dynamic dispatch and polymorphic routing scenarios
component: core
reviewed: 2024-04-04
redirects:
- nservicebus/messages-as-interfaces
- nservicebus/messaging/interfaces-as-messages
---

Events can be created on the fly from interfaces without first defining an explicit class implementing the interfaces. This is a useful technique to support multiple inheritance for [polymorphic routing scenarios](./dynamic-dispatch-and-routing.md) to make systems more resilient and maintainable over time.

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
