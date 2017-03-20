---
title: Service Fabric Persistence Transaction Sharing
reviewed: 2017-03-17
component: ServiceFabricPersistence
---

Service Fabric Persistence exposes current storage transaction via `IMessageHandlerContext.SynchronizedStorageSession` property. The transaction can be used to ensure atomicity of operations performed by business logic and the persister. This can be especially helpful when running endpoints with [Outbox](/nservicebus/outbox/) feature turned on.

snippet: ServiceFabricPersistenceSynchronizedSession