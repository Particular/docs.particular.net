---
title: Message Mutators
summary: Message Mutators allow mutation of messages in the pipeline
component: Core
reviewed: 2017-03-30
tags:
 - Mutator
redirects:
 - nservicebus/pipeline-management-using-message-mutators
related:
 - samples/messagemutators
 - nservicebus/messaging/headers
---

Message Mutators allow mutation of messages in the pipeline.

NServiceBus supports two categories of Message Mutators:


## Logical Message Mutators

Message mutators change/react to individual messages being sent or received. The `IMutateOutgoingMessages` or `IMutateIncomingMessages` interfaces allow the implementation of hooks for the sending and receiving sides.

Mutators can be used to perform actions such as validation of outgoing/incoming messages.


### IMutateIncomingMessages

snippet: IMutateIncomingMessages


### IMutateOutgoingMessages

snippet: IMutateOutgoingMessages


partial: imessagemutator


## Transport Messages Mutators

Transport message mutators work on the serialized transport message and are useful for compression, header manipulation, etc.
Create transport message mutators by implementing the `IMutateIncomingTransportMessages` or `IMutateOutgoingTransportMessages` interfaces.


### IMutateIncomingTransportMessages

snippet: IMutateIncomingTransportMessages


### IMutateOutgoingTransportMessages

snippet: IMutateOutgoingTransportMessages


partial: imutatetransportmessages


## Registering a Mutator

Mutators are registered using:

snippet: MutatorRegistration

NOTE: Mutators are **NOT** automatically registered using [dependency injection](/nservicebus/dependency-injection/) container, if DI is needed then use a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md).

NOTE: Mutators are non-deterministic in terms of order of execution. If more fine grained control is required over the pipeline see [Pipeline Introduction](/nservicebus/pipeline/manipulate-with-behaviors.md).


## When a mutator throws an exception

If a incoming mutator throws an exception, the message aborts, rolls back to the queue, and [recoverability](/nservicebus/recoverability/) is applied.

If an outgoing mutator throws an exception, the exception bubbles up to the method performing the Send or Publish. If the operation is performed on a context in the pipeline the message aborts, rolls back to the queue, and [recoverability](/nservicebus/recoverability/) is applied. If the operation is performed on the message session the exception might bubble up to the user code or tear down the application domain if not properly handled. 

partial: nonnulltask

include: mutators-versus-behaviors
