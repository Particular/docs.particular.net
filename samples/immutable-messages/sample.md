---
title: Immutable Messages samples
summary: Demonstrate how to use immutable messages when exchanging messages between endpoints.
reviewed: 2019-07-30
component: Core
related:
- nservicebus/messaging/immutable-messages
---

## Code walk-through

This sample shows how to define and exchange immutable messages between endpoints. Immutable messages can be defined as interfaces with only getters:

snippet: immutable-messages-as-interface

Or as classes with only getters and a non-default parameterless constructor:

snippet: immutable-messages-as-class


At startup the application creates two endpoint instances, a sender and a receiver. Both are configured to use Json.Net as serializer through the `NServiceBus.Newtonsoft.Json` package:

snippet: immutable-messages-endpoint-config

### Sending messages

The sender endpoint allows to send messages to the receiver using either an immutable message defined as a class or one defined as an interface. In this latter case the sender endpoint defines an internal message class that implements the public, shared, interface:

snippet: immutable-messages-as-interface-implementation

The internal class is then used by the sender endpoint at dispatch time

snippet: immutable-messages-as-interface-sending

NOTE: The class is compatible with *any* serializer if it has public getters and setters
### Receiving message

To receive an immutable message defined using one of the two presented techniques no special configuration is needed. The only requirement is a serializer capable, or configurable to do so, of deserializing objects using private setters and/or non-public parameterless constructors. The receiver endpoint defines regular message handlers:

snippet: immutable-messages-as-interface-handling

snippet: immutable-messages-as-class-handling
