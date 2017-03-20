---
title: Service Fabric Persistence Transaction Sharing
reviewed: 2017-03-20
component: ServiceFabricPersistence
---

Service Fabric Persistence exposes current storage transaction via `IMessageHandlerContext.SynchronizedStorageSession` property. The transaction can be used to ensure atomicity of operations performed by the business logic and the persister. This can be especially helpful when running endpoints with [Outbox](/nservicebus/outbox/) feature turned on.

snippet: ServiceFabricPersistenceSynchronizedSession

Warning: shared transaction should not be completed (`CommitAsync()`) or disposed (`Dispose()`) in the handler. In case data needs to be persisted to a custom collection during handler execution, a new transaction should be created using `session.StateManager`.

snippet: CustomTransaction