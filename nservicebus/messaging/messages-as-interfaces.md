---
title: Messages as Interfaces
summary: 'NServiceBus allows supports the use interfaces as well as standard XSD and class serialization.'
component: core
reviewed: 2017-03-14
redirects:
- nservicebus/messages-as-interfaces
---

NServiceBus support using interfaces as messages.

Using interfaces to means "multiple inheritance" is supported; i.e., one message can extend multiple other messages. This is useful for solving certain "message evolution" scenarios.

Say that the business logic represents a state machine with states X and Y. When the system gets into state X, it publishes the message `EnteredStateX`. When the system gets into state Y, it publishes the message `EnteredStateY`. (For more information on how to publish a message, see below.)

In the next version of the system, add a new state Z, which represents the co-existence of both X and Y. So, define the message `EnteredStateZ`, which inherits both `EnteredStateX` and `EnteredStateY`.

When the system publishes `EnteredStateZ`, clients subscribed to `EnteredStateX` and/or `EnteredStateY` are notified.

Without the ability to extend a message to multiple others, composition would be required, thereby preventing the infrastructure from automatically routing messages to pre-existing subscribers of the composed messages.


{{WARNING:

The following serializers **do not support** messages defined as interfaces. 

 * [Bond](/nservicebus/serialization/bond.md)
 * [MessagePack](/nservicebus/serialization/messagepack.md)
 * [MsgPack](/nservicebus/serialization/msgpack.md)
 * [Protobuf-Net](/nservicebus/serialization/protobufnet.md)
 * [Protobuf-Google](/nservicebus/serialization/protobufgoogle.md)
 * [Wire](/nservicebus/serialization/wire.md)
 * [ZeroFormatter](/nservicebus/serialization/zeroformatter.md)

Instead use a public class with the same contract as the interface. The class can optionally implement any required interfaces. 

}}


