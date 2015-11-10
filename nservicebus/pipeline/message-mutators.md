---
title: Message Mutators
summary: Message mutators allow mutation of messages in the pipeline.
tags: 
- Mutator
redirects:
- nservicebus/pipeline-management-using-message-mutators
related:
- samples/messagemutators
---

The message pipeline in NServiceBus V2.X consisted of message modules. They served their purpose but didn't quite give full control over the message pipeline for more advanced things, and there was no way to hook into the pipeline at the sending/client side of the message conversation.


## Two flavors of mutators

NServiceBus enables two types of message mutators:


### Message Instance Mutators

Message mutators change/react to individual messages being sent or received. The `IMessageMutator` interface lets you implement hooks for the sending and receiving sides. If you only need one, use the finely grained `IMutateOutgoingMessages` or `IMutateIncomingMessages`.

You can use reactions to individual messages to perform actions such as validation of outgoing/incoming messages.

NServiceBus uses this type of mutator internally to do things like property encryption and serialization/deserialization of properties to and from the DataBus.


### Transport Messages Mutators

Create transport message mutators by implementing the `IMutateTransportMessages` interface. This type of mutator works on the entire transport message and is useful for compression, header manipulation, etc. See a [full explanation of the syntax](/samples/messagemutators/).

Remember that message mutators are NOT automatically registered in the container, so to invoke them, register them in the container yourself.


## When should I use a message mutator?

Just like the recommendation for headers, only use message mutators for infrastructure purposes.

As a rule of thumb, consider using message mutators only to solve technical requirements.


## What happens if a mutator throws an exception?

If a server side (incoming) mutator throws an exception, the message aborts, rolls back to the queue, and is retried.

If a client side (outgoing) message mutator throws an exception, the exception bubbles up to the method calling `bus.Send` or `bus.Publish`.


NOTE: `IMutateTransportMessages` are non-deterministic in terms of order of execution. If you want more fine grained control over the pipeline see [Pipeline Introduction](/nservicebus/pipeline/customizing.md).