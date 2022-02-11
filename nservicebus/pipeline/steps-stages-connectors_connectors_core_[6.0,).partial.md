## Stage connectors

```mermaid
graph LR

subgraph Stage connectors
    A[TFromContext] --- B{Connector}
    B --- C[TToContext]
end
```

Stage connectors connect from the current stage (i.e. `IOutgoingLogicalMessageContext`) to another stage (i.e. `IOutgoingPhysicalMessageContext`). In order to override an existing stage, inherit from `StageConnector<TFromContext, TToContext>` and then replace an existing stage connector. Most pipeline extensions can be done by inheriting from `Behavior<TContext>`. It is rarely necessary to replace existing stage connectors. When implementing a stage connector, ensure that all required data is passed along for the next stage.

snippet: CustomStageConnector

## Fork Connectors

```mermaid
graph LR
subgraph Main pipeline
    A[TFromContext] --- B{Fork Connector}
    B --- C[TFromContext]
end
subgraph Fork pipeline
   B --> D[TForkContext]
end
```

Fork connectors fork from a current stage (i.e. `IIncomingPhysicalMessageContext`) to another independent pipeline (i.e. `IAuditContext`). In order to override an existing fork connector inherit from `ForkConnector<TFromContext, TForkContext>` and then replace an existing fork connector.

snippet: CustomForkConnector

## Stage Fork Connector

```mermaid
graph LR
subgraph Stage
    A[TFromContext] --- B{StageFork<br/>Connector}
    B --- C[TToContext]
end
subgraph Fork pipeline
   B --> D[TForkContext]
end
```

Stage fork connectors are essentially a combination of a stage connector and a fork connector. They have the ability to connect from the current stage (i.e. `ITransportReceiveContext`) to another stage (i.e. `IIncomingPhysicalMessageContext`) and fork to another independent pipeline (i.e. `IBatchedDispatchContext`). In order to override an existing stage fork connector inherit from `StageForkConnector<TFromContext, TToContext, TForkContext` and then replace an existing stage fork connector.

snippet: CustomStageForkConnector
