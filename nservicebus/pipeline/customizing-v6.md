---
title: Customizing the Pipeline in NServiceBus Version 6
summary: Customizing the message handling pipeline in NServiceBus Version 6.
tags:
- Pipeline
related:
- nservicebus/pipeline/customizing
---

NServiceBus version 6 splits the existing pipelines into smaller and better composable units called "Stages". A stage is a group of steps acting on the same level of abstraction. This allows you to define a step working with the "incoming physical" message before it has been deserialized or a step which is executed before and after each handler invocation (remember: there can be multiple message handlers per message). You'll find more information about the existing stages later in this article.

You can extend the pipeline with a custom behavior implementing `Behavior<TContext>`. `TContext` is the context of the stage that the behavior belongs to.

snippet:SamplePipelineBehavior

In the above code snippet the `SampleBehavior` class derives from `Behavior<IIncomingLogicalMessageContext>`. This tells the framework to execute this behavior after the incoming raw message has been deserialized and a matching message type has been found. At runtime, the pipeline will call the `Invoke` method of each registered behavior passing in as arguments the current message context and an action to invoke the next behavior in the pipeline.

Warning: Each behavior is responsible to call the next step in the pipeline chain by invoking `next`.


## Add a new step

To add your custom behavior to the pipeline, you need to define a step for it:

snippet:NewPipelineStep

Then register the new step in the pipeline settings:

snippet:AddPipelineStep


## Replace an existing step

To replace the implementation of an existing step, you can replace it with your custom behavior:

snippet:ReplacePipelineStep

Note: You can also register your steps from a [Feature](features.md)


## Stages

The following lists describe some of the common stages which you can build your own behaviors for. Each stage has a context associated with it (which is used by when implementing your custom behavior).


### Incoming Pipeline Stages

 * Physical message processing: Behaviors on this stage have access the raw message body before it is deserialized. This stage provides `IIncomingPhysicalMessageContext` to it's behaviors.
 * Logical message processing: This stage provides information about the received message type and it's deserialized instance. It provides `IIncomingLogicalMessageContext` to it's behaviors.
 * Handler invocation: Each received message can be handled by multiple handlers. This stage will be executed once for every associated handler and provides `IInvokeHandlerContext` to the behaviors.


### Outgoing Pipeline Stages

 * Operation specific processing: There is a dedicated stage for each bus operation (e.g. Send, Publish, Subscribe, ...). Behaviors can use one of the following contexts: `IOutgoingSendContext`, `IOutgoingPublishContext`, `IOutgoingReplyContext`, `ISubscribeContext`, `IUnsubscribeContext`.
 * Logical message processing: Behaviors on this stage have access to the message which should be sent. Use `IOutgoingLogicalMessageContext` in your behavior to enlist in this stage.
 * Audit message processing: In here behaviors have access to the message to be audited/sent to the audit queue and audit address. Behaviors should use `IAuditContext` to enlist in this stage.
 * Fault message processing: This is a dedicated stage for processing faulty messages that are being moved to the error queue. Behaviors have access to message, exception and error address. This stage provides `IFaultContext` to it's behaviors.
 * Physical message processing: Enables you to access the serialized message. This stage provides `IOutgoingPhysicalMessageContext` to it's behaviors.

NOTE: As in Version 5, you can still configure steps by ordering them using `WellKnownSteps`. We recommend to not rely on certain steps but to choose the appropriate stage instead.

## Sharing data between behaviors

Sometimes a parent behavior might need to pass some information to a child behavior and vice versa. The `context` parameter of a behavior's Invoke method facilitates  passing data between behaviors. The context is very similar to a shared dictionary which allows adding and retrieving information from different behaviors.

snippet:SharingBehaviorData

Note that the context respects the stage hierarchy and only allows adding new entries in the scope of the current context. A child behavior (later in the pipeline chain) can read and even modify entries set by a parent behavior (earlier in the pipeline chain) but entries added by the child cannot be accessed from the parent.

include: customizing-exception-handling

## Advanced concepts

### Stage Connectors

![Stage Connector](StageConnectors.svg)

Stage connectors connect from the current stage (i.ex. `IOutgoingLogicalMessageContext`) to another stage (i.ex. `IOutgoingPhysicalMessageContext`). In order to override an existing stage you need to inherit from `StageConnector<TFromContext, TToContext>` and then replace an existing stage connector. Most pipeline extensions can be done by just inheriting from `Behavior<TContext>`. It is rarely ever necessary to implement stage connectors or replace existing ones. If you implement a stage connector you need to make sure that all required data is passed along for the next stage.

snippet:CustomStageConnector

### Fork Connectors

![Fork Connector](ForkConnectors.svg)

Fork connectors fork from a current stage (i.ex. `IIncomingPhysicalMessageContext`) to another independent pipeline (i.ex. `IAuditContext`). A fork connector has the required knowledge to create additional pipelines and cache them appropriately for performance reasons. In order to override an existing fork connector you need to inherit from `ForkConnector<TFromContext, TForkContext>` and then replace an existing fork connector.

snippet:CustomForkConnector

There is currently no mechanism available to create pipelines which are not known to NServiceBus.

### Stage Fork Connector

![Stage Fork Connector](StageForkConnectors.svg)

Stage fork connectors are essentially a marriage of a stage connector and a fork connector. They have the ability to connect from the current stage (i.ex. `ITransportReceiveContext`) to another stage (i.ex. `IIncomingPhysicalMessageContext`) and fork to another independent pipeline (i.ex. `IBatchedDispatchContext`). Like a fork connector it has the required knowledge to create additional pipelines and cache them appropriately for performance reasons. In order to override an existing stage fork connector you need to inherit from `StageForkConnector<TFromContext, TToContext, TForkContext` and then replace an existing stage fork connector.

snippet:CustomStageForkConnector

There is currently no mechanism available to create pipelines which are not known to NServiceBus.
