### Recoverability pipeline stages

* Recoverability: Behaviors on this stage have access the raw message that failed. This stage provides `IRecoverabilityContext` to its behaviors, which exposes the endpoint's recoverability settings, the recoverability action (e.g. `MoveToErrorQueue`), as well as the `ErrorContext` and metadata.
* Routing: Provides access to the routing strategies that have been selected for the failed message. This stage provides `IRoutingContext` to its behaviors.
* Dispatch: Provides access to outgoing dispatch operations before they are handed off to the transport. This stage provides `IDispatchContext` to its behaviors.

``` mermaid
graph LR

Transport((Transport))
Transport2((Transport))

subgraph Recoverability Pipeline

TR[Recoverability]
RR[Routing]
Dispatch[Immediate<br>Dispatch]

end

Transport -- onError --> TR
TR -. 0-to-n .-> RR
RR --> Dispatch
Dispatch --> Transport2


click RR "/nservicebus/recoverability"
click Transport "/transports/"
```
