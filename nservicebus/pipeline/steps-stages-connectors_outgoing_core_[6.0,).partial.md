### Outgoing Pipeline Stages

* Operation specific processing: There is a dedicated stage for each context operation (e.g. Send, Publish, Subscribe, ...). Behaviors can use one of the following contexts: `IOutgoingSendContext`, `IOutgoingPublishContext`, `IOutgoingReplyContext`, `ISubscribeContext`, `IUnsubscribeContext`. Subscribe and Unsubscribe are not shown on the diagram below.
* Outgoing Logical Message: Behaviors on this stage have access to the message which should be sent. Use `IOutgoingLogicalMessageContext` in a behavior to enlist in this stage.
* Outgoing Physical Message: Enables to access the serialized message. This stage provides an `IOutgoingPhysicalMessageContext` instance to its behaviors.
* Routing: Allows the selected routing strategies for outgoing messages to be manipulated. If the outgoing pipeline was initiated by the incoming pipeline, this stage collects [all outgoing operations](/nservicebus/messaging/batched-dispatch.md) (except for [immediate dispatch messages](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately)).
This stage provides an `IRoutingContext` instance to its behaviors.
* Batch Dispatch: Forwards all [collected outgoing operations](/nservicebus/messaging/batched-dispatch.md) to the dispatch stage once message processing has been completed. This stage provides access to the collection of transport operations that are to be dispatched. This stage provides an `IBatchDispatchContext` instance to its behaviors.
* Dispatch: Provides access to outgoing dispatch operations before they are handed off to the transport. This stage provides an `IDispatchContext` instance to its behaviors.

```mermaid
graph LR
UC((Initiating User Code))

subgraph Outgoing Pipeline
Outgoing{Outgoing}
OP[Outgoing<br>Publish]
OS[Outgoing<br>Send]
OR[Outgoing<br>Reply]
OLM[Outgoing<br>Logical<br>Message]
OPM[Outgoing<br>Physical<br>Message]
Routing[Routing]
end


subgraph Dispatch
Dispatch{Dispatch}
Transport((Transport))
BD[Batch<br>Dispatch]
ID[Immediate<br>Dispatch]
end

UC --> Outgoing
Outgoing --> OP
Outgoing --> OS
Outgoing --> OR
OP --> OLM
OS --> OLM
OR --> OLM
OLM --> OPM
OPM --> Routing
Routing --> Dispatch

ID--> Transport
Dispatch --> BD
Dispatch --> ID
BD --> Transport
```
