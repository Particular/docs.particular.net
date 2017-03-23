---
title: Service Fabric Persistence Transaction Sharing
reviewed: 2017-03-20
component: ServiceFabricPersistence
---

The current storage transaction is exposed via the `SynchronizedStorageSession` property on the `IMessageHandlerContext` implementation. The transaction can be used to ensure atomicity of operations performed by both the business logic and the persister. When running endpoints with the [Outbox](/nservicebus/outbox/) feature turned on the same transaction will be used for any outgoing messages as well.

### Using in a Handler

snippet: ServiceFabricPersistenceSynchronizedSession-Handler

### Using in a Saga

include: saga-business-data-access

snippet: ServiceFabricPersistenceSynchronizedSession-Saga

Warning: Shared transaction should not be committed (`CommitAsync()`) or disposed (`Dispose()`) in the handler or the saga. When data needs to be persisted to a custom collection during handler or saga execution, a new transaction should be created using the `StateManager` property of the incoming session.

snippet: CustomTransaction