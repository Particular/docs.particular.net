NServiceBus Version 6 further splits the existing pipelines into smaller composable units called "Stages" and "Connectors". A stage is a group of steps acting on the same level of abstraction.


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

Stage connectors connect from the current stage (i.e. `IOutgoingLogicalMessageContext`) to another stage (i.e. `IOutgoingPhysicalMessageContext`). In order to override an existing stage to inherit from `StageConnector<TFromContext, TToContext>` and then replace an existing stage connector. Most pipeline extensions can be done by inheriting from `Behavior<TContext>`. It is rarely ever necessary to implement stage connectors or replace existing ones. When implementing a stage connector ensure that all required data is passed along for the next stage.

snippet:CustomStageConnector


## Fork Connectors

![Fork Connector](fork-connectors.svg)

Fork connectors fork from a current stage (i.e. `IIncomingPhysicalMessageContext`) to another independent pipeline (i.e. `IAuditContext`). A fork connector has the required knowledge to create additional pipelines and cache them appropriately for performance reasons. In order to override an existing fork connector inherit from `ForkConnector<TFromContext, TForkContext>` and then replace an existing fork connector.

snippet:CustomForkConnector

There is currently no mechanism available to create pipelines which are not known to NServiceBus.


## Stage Fork Connector

![Stage Fork Connector](stage-fork-connectors.svg)

Stage fork connectors are essentially a marriage of a stage connector and a fork connector. They have the ability to connect from the current stage (i.e. `ITransportReceiveContext`) to another stage (i.e. `IIncomingPhysicalMessageContext`) and fork to another independent pipeline (i.e. `IBatchedDispatchContext`). Like a fork connector it has the required knowledge to create additional pipelines and cache them appropriately for performance reasons. In order to override an existing stage fork connector inherit from `StageForkConnector<TFromContext, TToContext, TForkContext` and then replace an existing stage fork connector.

snippet:CustomStageForkConnector

Note: There is currently no mechanism available to create pipelines which are not known to NServiceBus.