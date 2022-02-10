### Outgoing Pipeline Stages

* Operation specific processing: There is a dedicated stage for each context operation (e.g. Send, Publish, Subscribe, ...). Behaviors can use one of the following contexts: `IOutgoingSendContext`, `IOutgoingPublishContext`, `IOutgoingReplyContext`, `ISubscribeContext`, `IUnsubscribeContext`. Subscribe and Unsubscribe are not shown on the diagram below.
* Outgoing Logical Message: Behaviors on this stage have access to the message which should be sent. Use `IOutgoingLogicalMessageContext` in a behavior to enlist in this stage.
* Outgoing Physical Message: Enables to access the serialized message. This stage provides `IOutgoingPhysicalMessageContext` to it's behaviors.
* Routing: Provides access to the routing strategies that have been selected for the outgoing message. This stage provides `IRoutingContext` to it's behaviors.
* Batch Dispatch: when messages are sent as part of a message handler or saga handler, or when using `IMessageSession` to send messages. Outgoing messages are [collected into a batch](/nservicebus/messaging/batched-dispatch.md) and handed to the Batch Dispatch stage all at once once message processing has been completed. This stage provides access to the collection of transport operations that are to be dispatched. This stage provides `IBatchDispatchContext` to it's behaviors. The batch dispatch stage can be bypassed by specifying [immediate dispatch](/nservicebus/messaging/send-a-message.md) for an outgoing message.
* Dispatch: provides access to outgoing dispatch operations before they are handed off to the transport. This stage provides `IDispatchContext` to it's behaviors.

```mermaid
graph LR
UC[Initiating<br>User Code]

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
Transport[Transport]
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