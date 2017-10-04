---
title: Manipulate Pipeline with Behaviors
summary: Manipulating the message handling pipeline with Behaviors
component: Core
versions: '[4.0,)'
reviewed: 2017-04-17
tags:
- Pipeline
related:
- nservicebus/pipeline/steps-stages-connectors
redirects:
- nservicebus/pipeline/customizing
- nservicebus/pipeline/customizing-v5
- nservicebus/pipeline/customizing-v6
---

Pipelines are made up of a group of steps acting on the same level of abstraction. This allows scenarios such as

 * Defining a step that works with the "incoming physical" message before it has been deserialized.
 * Defining a step that is executed before and after each handler invocation (remember: there can be multiple message handlers per message).

Extending the pipeline is done with a custom behavior implementing `Behavior<TContext>`.`TContext` is the context of the stage that the behavior belongs to.

snippet: SamplePipelineBehavior

In the above code snippet the `SampleBehavior` class derives from the Behavior contract and targets the incoming context. This tells the framework to execute this behavior after the incoming raw message has been deserialized and a matching message type has been found. At runtime, the pipeline will call the `Invoke` method of each registered behavior passing in as arguments the current message context and an action to invoke the next behavior in the pipeline.

Warning: Each behavior is responsible to call the next step in the pipeline chain by invoking `next()`.


## Add a new step

partial: registernew


## Replace an existing step

To replace the implementation of an existing step replace it with a custom behavior:

snippet: ReplacePipelineStep

In order to replace the existing step it is necessary to provide a step id. The most reliable way of determining the step id, is to find the step definition in the NServiceBus source code. 

Note, however, that step ids are hard-coded strings and may change in the future resulting in an unexpected behavior change. In case of replacing built-in steps, create automatic tests that will detect potential id change or step removal.

Note: Steps can also be registered from a [Feature](features.md).


## Exception Handling

Exceptions thrown from a behavior's `Invoke` method bubble up the chain. If the exception is not handled by a behavior, the message is considered as faulted which results in putting the message back in the queue (and rolling back the transaction) or moving it to the error queue (depending on the endpoint configuration).


partial: messagedeserializationexception


partial: skipserialization


partial: shareddata


partial: sharedcontext


include: mutators-versus-behaviors
