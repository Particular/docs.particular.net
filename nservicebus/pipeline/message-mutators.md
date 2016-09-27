---
title: Message Mutators
summary: Message Mutators allow mutation of messages in the pipeline
component: Core
reviewed: 2016-08-23
tags:
- Mutator
redirects:
- nservicebus/pipeline-management-using-message-mutators
related:
- samples/messagemutators
- nservicebus/messaging/headers
---

Message Mutators allow mutation of messages in the pipeline.


## Two flavors of Mutators

NServiceBus enables two categories of Message Mutators:


### Logical Message Mutators

Message mutators change/react to individual messages being sent or received. The `IMutateOutgoingMessages` or `IMutateIncomingMessages` interfaces allow the implementation of hooks for the sending and receiving sides.

Mutators can be used to perform actions such as validation of outgoing/incoming messages.

NServiceBus uses this type of mutator internally to do things like property encryption and serialization/deserialization of properties to and from the DataBus.


#### IMutateIncomingMessages

snippet:IMutateIncomingMessages


#### IMutateOutgoingMessages

snippet:IMutateOutgoingMessages


partial: imessagemutator


### Transport Messages Mutators

Create transport message mutators by implementing the `IMutateIncomingTransportMessages` or `IMutateOutgoingTransportMessages` interfaces. This type of mutator works on the entire transport message and is useful for compression, header manipulation, etc.


#### IMutateIncomingTransportMessages

snippet:IMutateIncomingTransportMessages


#### IMutateOutgoingTransportMessages

snippet:IMutateOutgoingTransportMessages


partial: imutatetransportmessages


## Registering a Mutator

Mutators are **NOT** automatically registered in the container, so to have them invoked, register them in the [container](/nservicebus/containers/).

snippet:MutatorRegistration

NOTE: Mutators are non-deterministic in terms of order of execution. If more fine grained control is required over the pipeline see [Pipeline Introduction](/nservicebus/pipeline/manipulate-with-behaviors.md).


## When a mutator throws an exception

If a incoming throws an exception, the message aborts, rolls back to the queue, and [recoverability](/nservicebus/recoverability/) is applied.

If a outgoing mutator throws an exception, the exception bubbles up to the method performing the Send or Publish.

partial: nonnulltask

include: mutators-versus-behaviors