---
title: Pipeline Steps, Stages and Connectors
summary: The pipeline is composed of a number of Stages that communicate via Connectors.
tags:
- Pipeline
- Connector
- Stage
related:
- nservicebus/pipeline/manipulate-with-behaviors
---


NServiceBus has the concept of a "pipeline" which refers to the series of actions taken when an incoming message is process and an outgoing message is sent.

NServiceBus has always had the concept of a pipeline execution order that is executed when a message is received or dispatched. From NServiceBus Version 5 on, this pipeline has been made a first level concept and exposes it for extensibility. This allows users to take full control of the incoming and outgoing message processing.

In NServiceBus there are two explicit pipelines: one for the outgoing messages and one for the incoming messages.

Each pipeline is composed of "steps". A step is an identifiable value in the pipeline used to programmatically define order of execution. Each step represents a behavior which will be executed at the given place within the pipeline. To add additional behavior to the pipeline by registering a new step or replace the behavior of an existing step with the custom implementation.

NServiceBus Version 6 further splits the existing pipelines into smaller composable units called "Stages" and "Connectors". A stage is a group of steps acting on the same level of abstraction.


WARNING: The following concepts are specific to NServiceBus Versions 6 and above.


## Stages

The following lists describe some of the common stages that behaviors can be built for. Each stage has a context associated with it (which is used by when implementing a custom behavior).


### Incoming Pipeline Stages

 * Physical message processing: Behaviors on this stage have access the raw message body before it is deserialized. This stage provides `IIncomingPhysicalMessageContext` to it's behaviors.
 * Logical message processing: This stage provides information about the received message type and it's deserialized instance. It provides `IIncomingLogicalMessageContext` to it's behaviors.
 * Handler invocation: Each received message can be handled by multiple handlers. This stage will be executed once for every associated handler and provides `IInvokeHandlerContext` to the behaviors.


### Outgoing Pipeline Stages

 * Operation specific processing: There is a dedicated stage for each bus operation (e.g. Send, Publish, Subscribe, ...). Behaviors can use one of the following contexts: `IOutgoingSendContext`, `IOutgoingPublishContext`, `IOutgoingReplyContext`, `ISubscribeContext`, `IUnsubscribeContext`.
 * Logical message processing: Behaviors on this stage have access to the message which should be sent. Use `IOutgoingLogicalMessageContext` in a behavior to enlist in this stage.
 * Audit message processing: In here behaviors have access to the message to be audited/sent to the audit queue and audit address. Behaviors should use `IAuditContext` to enlist in this stage.
 * Fault message processing: This is a dedicated stage for processing faulty messages that are being moved to the error queue. Behaviors have access to message, exception and error address. This stage provides `IFaultContext` to it's behaviors.
 * Physical message processing: Enables to access the serialized message. This stage provides `IOutgoingPhysicalMessageContext` to it's behaviors.



## Stage Connectors

![Stage Connector](stage-connectors.svg)

Stage connectors connect from the current stage (i.ex. `IOutgoingLogicalMessageContext`) to another stage (i.ex. `IOutgoingPhysicalMessageContext`). In order to override an existing stage to inherit from `StageConnector<TFromContext, TToContext>` and then replace an existing stage connector. Most pipeline extensions can be done by inheriting from `Behavior<TContext>`. It is rarely ever necessary to implement stage connectors or replace existing ones. When implementing a stage connector ensure that all required data is passed along for the next stage.

snippet:CustomStageConnector


## Fork Connectors

![Fork Connector](fork-connectors.svg)

Fork connectors fork from a current stage (i.ex. `IIncomingPhysicalMessageContext`) to another independent pipeline (i.ex. `IAuditContext`). A fork connector has the required knowledge to create additional pipelines and cache them appropriately for performance reasons. In order to override an existing fork connector inherit from `ForkConnector<TFromContext, TForkContext>` and then replace an existing fork connector.

snippet:CustomForkConnector

There is currently no mechanism available to create pipelines which are not known to NServiceBus.


## Stage Fork Connector

![Stage Fork Connector](stage-fork-connectors.svg)

Stage fork connectors are essentially a marriage of a stage connector and a fork connector. They have the ability to connect from the current stage (i.ex. `ITransportReceiveContext`) to another stage (i.ex. `IIncomingPhysicalMessageContext`) and fork to another independent pipeline (i.ex. `IBatchedDispatchContext`). Like a fork connector it has the required knowledge to create additional pipelines and cache them appropriately for performance reasons. In order to override an existing stage fork connector inherit from `StageForkConnector<TFromContext, TToContext, TForkContext` and then replace an existing stage fork connector.

snippet:CustomStageForkConnector

Note: There is currently no mechanism available to create pipelines which are not known to NServiceBus.