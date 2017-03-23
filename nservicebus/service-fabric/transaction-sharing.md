---
title: Service Fabric Persistence Transaction Sharing
reviewed: 2017-03-20
component: ServiceFabricPersistence
---

Service Fabric Persistence exposes the current storage transaction via the `SynchronizedStorageSession` property on the `IMessageHandlerContext` implementation. The transaction can be used to ensure atomicity of operations performed by both the business logic and the persister. This can be especially helpful when running endpoints with the [Outbox](/nservicebus/outbox/) feature turned on.

### Using in a Handler

snippet: ServiceFabricPersistenceSynchronizedSession-Handler

### Using in a Saga

include: saga-business-data-access

snippet: ServiceFabricPersistenceSynchronizedSession-Saga

Warning: shared transaction should not be completed (`CommitAsync()`) or disposed (`Dispose()`) in the handler. In case data needs to be persisted to a custom collection during handler execution, a new transaction should be created using the `StateManager` property of the incoming session.

snippet: CustomTransaction