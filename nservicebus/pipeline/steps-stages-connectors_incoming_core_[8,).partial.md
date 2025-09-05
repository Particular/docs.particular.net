### Incoming pipeline stages

* Transport Receive: Behaviors in this stage are responsible for signaling failure or success to the transport. In this stage no outgoing operations are supported. This stage provides `ITransportReceiveContext` to its behaviors.
* Incoming Physical Message: Behaviors on this stage have access to the raw message body before it is deserialized. This stage provides `IIncomingPhysicalMessageContext` to its behaviors.
* Incoming Logical Message: This stage provides information about the received message type and its deserialized instance. It provides `IIncomingLogicalMessageContext` to its behaviors.
* Invoke Handlers: Each received message can be handled by multiple handlers. This stage will be executed once for every associated handler and provides `IInvokeHandlerContext` to the its behaviors.

The connection between the Incoming Physical Message stage and the audit stages is an example of a fork. The message will flow down the main path and then down the fork path. The fork paths are only followed if the audit feature has been enabled.

```mermaid
graph LR

Transport((Transport))

subgraph Incoming Pipeline
TR[Transport<br>Receive]
IH[Invoke<br>Handler]
ILM[Incoming<br>Logical<br>Message]
IPM[Incoming<br>Physical<br>Message]
RUC((Receiving User Code))

subgraph Ancillary Actions
Audit[Audit]
Routing
Dispatch
end

end

AncillaryTransport[Transport]

Transport -- onMessage --> TR
IPM -- Step 1 --> ILM
IPM -. Step 2 .-> Audit
ILM --> IH

TR --> IPM
IH --> RUC

Dispatch --> AncillaryTransport
Audit --> Routing
Routing --> Dispatch

click IH "/nservicebus/handlers-and-sagas"
click Transport "/transports/"
click Audit "/nservicebus/operations/auditing"
```
