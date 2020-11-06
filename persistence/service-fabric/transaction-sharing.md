---
title: Service Fabric Persistence Transaction Sharing
reviewed: 2020-11-06
component: ServiceFabricPersistence
---

The current storage transaction is exposed via the `SynchronizedStorageSession` property on the `IMessageHandlerContext` implementation. The transaction can be used to ensure atomicity of operations performed by both the business logic and the persister. When running endpoints with the [Outbox](/nservicebus/outbox/) feature turned on; the same transaction will be used for any outgoing messages as well.


### Using in a Handler

snippet: ServiceFabricPersistenceSynchronizedSession-Handler


### Using in a Saga

include: saga-business-data-access

snippet: ServiceFabricPersistenceSynchronizedSession-Saga

Warning: Shared transaction should not be committed (`CommitAsync()`) or disposed (`Dispose()`) in the handler or the saga. 

### Using a custom transaction

When the data needs to be persisted in a custom collection during the handler or saga execution, the action should not participate in the provided transaction. Instead, a new transaction should be created using the `StateManager` property of the incoming session. This is useful when the information needs to be stored regardless of the outcome of the handler's execution.

Service Fabric transactions should be as short-lived as possible and touch only a single resource to reduce lock contention and avoid timeouts.

snippet: CustomTransaction
