---
title: Manipulate Pipeline with Behaviors
summary: Manipulating the message handling pipeline with Behaviors
component: Core
versions: '[4.0,)'
reviewed: 2016-08-24
tags:
- Pipeline
related:
- nservicebus/pipeline/steps-stages-connectors
redirects:
- nservicebus/pipeline/customizing
- nservicebus/pipeline/customizing-v5
- nservicebus/pipeline/customizing-v6
---

NServiceBus pipelines are made up of a group of steps acting on the same level of abstraction. This allows scenarios such as

 * Defining a step that works with the "incoming physical" message before it has been deserialized.
 * Defining a step that is executed before and after each handler invocation (remember: there can be multiple message handlers per message).

Extending the pipeline is done with a custom behavior implementing `Behavior<TContext>`.`TContext` is the context of the stage that the behavior belongs to.

snippet:SamplePipelineBehavior

In the above code snippet the `SampleBehavior` class derives from the Behavior contract and target the incoming context. This tells the framework to execute this behavior after the incoming raw message has been deserialized and a matching message type has been found. At runtime, the pipeline will call the `Invoke` method of each registered behavior passing in as arguments the current message context and an action to invoke the next behavior in the pipeline.

Warning: Each behavior is responsible to call the next step in the pipeline chain by invoking `next()`.


## Add a new step

To add a custom behavior to the pipeline define a step for it:

snippet:NewPipelineStep


partial:registernew


## Replace an existing step

To replace the implementation of an existing step replace it with a custom behavior:

snippet:ReplacePipelineStep

Note: Steps can also be registered from a [Feature](features.md).


## Exception Handling

Exceptions thrown from a behaviors `Invoke` method bubble up the chain. If the exception is not handled by a behavior, the message is considered as faulted which results in putting the message back in the queue (and rolling back the transaction) or moving it to the error queue (depending on the endpoint configuration).


partial: messagedeserializationexception


## Skip Serialization

When writing extensions to the pipeline it may be necessary to either take control of the serialization or to skip it entirely. One example usage of this is the [Callbacks](/nservicebus/messaging/handling-responses-on-the-client-side.md). Callbacks skips serialization for integers and enums and instead embeds them in the message headers.

To skip serialization implement a behavior that targets `IOutgoingLogicalMessageContext`. For example the following behavior skips serialization if a send on an integer is requested. It instead places that in the header.

snippet: SkipSerialization

On the receiving side this header can then be extracted from the headers and decisions on the incoming message processing pipeline can be made based on it.


## Sharing data between Behaviors

Sometimes a parent behavior might need to pass some information to a child behavior and vice versa. The `context` parameter of a behavior's Invoke method facilitates passing data between behaviors. The context is very similar to a shared dictionary which allows adding and retrieving information from different behaviors.

snippet:SharingBehaviorData


partial: sharedcontext