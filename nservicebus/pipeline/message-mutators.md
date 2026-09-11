---
title: Message Mutators
summary: Message Mutators allow mutation of messages in the pipeline
component: Core
reviewed: 2026-09-11
redirects:
 - nservicebus/pipeline-management-using-message-mutators
related:
 - samples/messagemutators
 - nservicebus/messaging/headers
 - nservicebus/messaging/trimming-safe-messaging-overloads
---

Message mutators allow mutation of messages in the pipeline.

NServiceBus supports two categories of message mutators:


## Logical message mutators

Message mutators change/react to individual messages being sent or received. The `IMutateOutgoingMessages` or `IMutateIncomingMessages` interfaces allow the implementation of hooks for the sending and receiving sides.

Mutators can be used to perform actions such as validation of outgoing/incoming messages.


### IMutateIncomingMessages

snippet: IMutateIncomingMessages


### IMutateOutgoingMessages

snippet: IMutateOutgoingMessages


### Replacing the message instance

Starting in NServiceBus version 10.3, logical message mutators can replace the message instance by calling a strongly-typed method. The typed method keeps the logical message type known at compile time, which is required for [trimming and Native AOT](/nservicebus/messaging/trimming-safe-messaging-overloads.md#logical-message-mutators).

| Mutator context | Replace the message instance |
| -- | -- |
| `MutateIncomingMessageContext` | `context.UpdateMessageInstance(newMessage)` |
| `MutateOutgoingMessageContext` | `context.UpdateMessage(newMessage)` |

Both contexts also provide an overload that accepts the message instance and an explicit `Type` for scenarios where the message type is not known at compile time.

The declared type is the logical message type. For outgoing messages, it also determines how the message is routed and which message type is recorded on the message. It can differ from the runtime type of the instance as long as the instance is assignable to the declared type.

> [!NOTE]
> Assigning to `MutateIncomingMessageContext.Message` or `MutateOutgoingMessageContext.OutgoingMessage` determines the logical message type from the runtime type of the instance, which is not trimming-safe. Starting in version 10.3, these setters are obsolete and the compiler reports a warning when they are used. They will be treated as errors from version 11. The getters remain available and are the recommended way to read the current message.


## Transport message mutators

Transport message mutators work on the serialized transport message and are useful for compression, header manipulation, etc.
Create transport message mutators by implementing the `IMutateIncomingTransportMessages` or `IMutateOutgoingTransportMessages` interfaces.


### IMutateIncomingTransportMessages

snippet: IMutateIncomingTransportMessages


### IMutateOutgoingTransportMessages

snippet: IMutateOutgoingTransportMessages


## Registering a mutator

Mutators are registered using:

snippet: MutatorRegistration

> [!NOTE]
> Mutators are non-deterministic in terms of order of execution. If more fine-grained control is required over the pipeline see [Pipeline Introduction](/nservicebus/pipeline/manipulate-with-behaviors.md).


## When a mutator throws an exception

If an incoming mutator throws an exception, the message aborts, rolls back to the queue, and [recoverability](/nservicebus/recoverability/) is applied.

If an outgoing mutator throws an exception, the exception bubbles up to the method performing the Send or Publish. If the operation is performed on a context in the pipeline the message aborts, rolls back to the queue, and [recoverability](/nservicebus/recoverability/) is applied. If the operation is performed on the message session the exception might bubble up to the user code or tear down the application domain if not properly handled.


include: non-null-task

include: mutators-versus-behaviors
