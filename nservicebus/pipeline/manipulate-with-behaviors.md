---
title: Manipulate Pipeline with Behaviors
summary: Manipulating the message handling pipeline with behaviors
component: Core
versions: '[4.0,)'
reviewed: 2018-08-30
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

Extending the pipeline is done with a custom behavior implementing `Behavior<TContext>`. `TContext` is the context of the stage that the behavior belongs to. A list of possible pipeline stages a behavior can be attached to can be found in [Steps, Stages, and Connectors](steps-stages-connectors.md).

snippet: SamplePipelineBehavior

In the above code snippet the `SampleBehavior` class derives from the Behavior contract and targets the incoming context. This tells the framework to execute this behavior after the incoming raw message has been deserialized and a matching message type has been found. At runtime, the pipeline will call the `Invoke` method of each registered behavior passing in as arguments the current message context and an action to invoke the next behavior in the pipeline.

Warning: Each behavior is responsible to call the next step in the pipeline chain by invoking `next()`.


## Add a new step

partial: registernew


## Replace an existing step

To replace the implementation of an existing step, substitute it with a custom behavior:

snippet: ReplacePipelineStep

In order to replace the existing step, it is necessary to provide a step ID. The most reliable way of determining the step ID, is to find the step definition in the NServiceBus source code. 

Note, however, that step IDs are hard-coded strings and may change in the future resulting in an unexpected behavior change. When replacing built-in steps, create automatic tests that will detect potential ID changes or step removal.

Note: Steps can also be registered from a [feature](features.md).


## Exception handling

Exceptions thrown from a behavior's `Invoke` method bubble up the chain. If the exception is not handled by a behavior, the message is considered as faulted which results in putting the message back in the queue (and rolling back the transaction) or moving it to the error queue (depending on the endpoint configuration).


### MessageDeserializationException

If a message fails to deserialize a `MessageDeserializationException` will be thrown by the `DeserializeLogicalMessagesBehavior`. In this case, the message is directly moved to the error queue to avoid blocking the system by poison messages.


partial: skipserialization


## Sharing data between Behaviors

Sometimes a parent behavior might need to pass some information to a child behavior and vice versa. The `context` parameter of a behavior's `Invoke` method facilitates passing data between behaviors. The context is very similar to a shared dictionary which allows adding and retrieving information from different behaviors.

snippet: SharingBehaviorData

Note: Contexts are not concurrency safe.


partial: sharedcontext


include: mutators-versus-behaviors
