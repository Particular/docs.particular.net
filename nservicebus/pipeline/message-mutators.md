---
title: Message Mutators
summary: Message mutators allow mutation of messages in the pipeline.
tags:
- Mutator
redirects:
- nservicebus/pipeline-management-using-message-mutators
related:
- samples/messagemutators
- nservicebus/messaging/headers
---

The message pipeline in NServiceBus V2.X consisted of message modules. They served their purpose but didn't quite give full control over the message pipeline for more advanced things, and there was no way to hook into the pipeline at the sending/client side of the message conversation.


## Two flavors of mutators

NServiceBus enables two types of message mutators:


### Logical Message Mutators

Message mutators change/react to individual messages being sent or received. The `IMessageMutator` interface allows the implementation of hooks for the sending and receiving sides. If more fine grained is required use `IMutateOutgoingMessages` or `IMutateIncomingMessages`.

Mutators can be used to perform actions such as validation of outgoing/incoming messages.

NServiceBus uses this type of mutator internally to do things like property encryption and serialization/deserialization of properties to and from the DataBus.


#### IMutateIncomingMessages

snippet:IMutateIncomingMessages


#### IMutateOutgoingMessages

snippet:IMutateOutgoingMessages


#### IMessageMutator

`IMessageMutator` is an interface that combines both `IMutateIncomingMessages` and `IMutateOutgoingMessages`. It only exists in Versions 5 and below. In Versions 6 and above implement both `IMutateIncomingMessages` and `IMutateOutgoingMessages` instead.


### Transport Messages Mutators

Create transport message mutators by implementing the `IMutateTransportMessages` interface. This type of mutator works on the entire transport message and is useful for compression, header manipulation, etc.


#### IMutateIncomingTransportMessages

snippet:IMutateIncomingTransportMessages


#### IMutateOutgoingTransportMessages

snippet:IMutateOutgoingTransportMessages


#### IMutateTransportMessages

`IMutateTransportMessages` is an interface that combines both `IMutateIncomingTransportMessages` and `IMutateOutgoingTransportMessages`. It only exists in Versions 5 and below. In Versions 6 and above implement both `IMutateTransportMessages` and `IMutateOutgoingTransportMessages` instead.


## Registering a Mutator

Mutators are **NOT** automatically registered in the container, so to have them invoked, register them in the [container](/nservicebus/containers/).

snippet:MutatorRegistration

NOTE: Mutators are non-deterministic in terms of order of execution. If more fine grained control is required over the pipeline see [Pipeline Introduction](/nservicebus/pipeline/manipulate-with-behaviors.md).


## When a mutator throws an exception

If a incoming throws an exception, the message aborts, rolls back to the queue, and [error handling](/nservicebus/errors/) is applied.

If a outgoing mutator throws an exception, the exception bubbles up to the method performing the Send or Publish.

include: non-null-task