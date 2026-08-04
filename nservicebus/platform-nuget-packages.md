---
title: Platform NuGet packages
summary: A curated list of NuGet packages commonly used to build a distributed system with NServiceBus and the Particular Service Platform
reviewed: 2026-07-31
related:
 - nservicebus/upgrades/support-policy
 - nservicebus/upgrades/supported-platforms
---

This page contains a curated list of NuGet packages commonly used to build a distributed system with NServiceBus and the Particular Service Platform.

Every endpoint references the core package, which provides the messaging, routing, and recoverability behavior that the other packages extend:

- [NServiceBus](https://www.nuget.org/packages/NServiceBus)

### Transports

The transport is the messaging infrastructure that carries messages between endpoints. Each endpoint is configured with one:

- [NServiceBus.Transport.AzureServiceBus](https://www.nuget.org/packages/NServiceBus.Transport.AzureServiceBus)
- [NServiceBus.Transport.AzureStorageQueues](https://www.nuget.org/packages/NServiceBus.Transport.AzureStorageQueues)
- [NServiceBus.AmazonSQS](https://www.nuget.org/packages/NServiceBus.AmazonSQS)
- [NServiceBus.RabbitMQ](https://www.nuget.org/packages/NServiceBus.RabbitMQ)
- [NServiceBus.Transport.SqlServer](https://www.nuget.org/packages/NServiceBus.Transport.SqlServer)
- [NServiceBus.Transport.PostgreSql](https://www.nuget.org/packages/NServiceBus.Transport.PostgreSql)
- [NServiceBus.Transport.Msmq](https://www.nuget.org/packages/NServiceBus.Transport.Msmq)
- [NServiceBus.Transport.IBMMQ](https://www.nuget.org/packages/NServiceBus.Transport.IBMMQ)

### Persistence

Persistence stores saga state and outbox data, and supplies subscription or delayed delivery storage for transports that have no native support for them. See [selecting a persister](/persistence/selecting.md) for help choosing:

- [NServiceBus.Persistence.Sql](https://www.nuget.org/packages/NServiceBus.Persistence.Sql)
- [NServiceBus.Persistence.CosmosDB](https://www.nuget.org/packages/NServiceBus.Persistence.CosmosDB)
- [NServiceBus.Persistence.DynamoDB](https://www.nuget.org/packages/NServiceBus.Persistence.DynamoDB)
- [NServiceBus.Persistence.AzureTable](https://www.nuget.org/packages/NServiceBus.Persistence.AzureTable)
- [NServiceBus.Storage.MongoDB](https://www.nuget.org/packages/NServiceBus.Storage.MongoDB)
- [NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB)
- [NServiceBus.Persistence.ServiceFabric](https://www.nuget.org/packages/NServiceBus.Persistence.ServiceFabric)
- [NServiceBus.NHibernate](https://www.nuget.org/packages/NServiceBus.NHibernate)
- [NServiceBus.Persistence.NonDurable](https://www.nuget.org/packages/NServiceBus.Persistence.NonDurable)

### Hosting

These packages integrate NServiceBus with a .NET generic host, a serverless platform, or the `Microsoft.Extensions` abstractions:

- [NServiceBus.Extensions.Hosting](https://www.nuget.org/packages/NServiceBus.Extensions.Hosting)
- [NServiceBus.Extensions.DependencyInjection](https://www.nuget.org/packages/NServiceBus.Extensions.DependencyInjection)
- [NServiceBus.Extensions.Logging](https://www.nuget.org/packages/NServiceBus.Extensions.Logging)
- [NServiceBus.AzureFunctions.InProcess.ServiceBus](https://www.nuget.org/packages/NServiceBus.AzureFunctions.InProcess.ServiceBus)
- [NServiceBus.AzureFunctions.Worker.ServiceBus](https://www.nuget.org/packages/NServiceBus.AzureFunctions.Worker.ServiceBus)
- [NServiceBus.AwsLambda.Sqs](https://www.nuget.org/packages/NServiceBus.AwsLambda.Sqs)

### Serialization

The serializer controls the wire format of message bodies. The System.Text.Json and XML serializers ship in the core package, so a separate package is only needed for another format:

- [NServiceBus.Newtonsoft.Json](https://www.nuget.org/packages/NServiceBus.Newtonsoft.Json)

### Monitoring

These packages send health, metrics, and audit data to ServiceControl, so that ServicePulse can report on the running system:

- [NServiceBus.Metrics](https://www.nuget.org/packages/NServiceBus.Metrics)
- [NServiceBus.Metrics.ServiceControl](https://www.nuget.org/packages/NServiceBus.Metrics.ServiceControl)
- [NServiceBus.Metrics.ServiceControl.Msmq](https://www.nuget.org/packages/NServiceBus.Metrics.ServiceControl.Msmq)
- [NServiceBus.Metrics.PerformanceCounters](https://www.nuget.org/packages/NServiceBus.Metrics.PerformanceCounters)
- [NServiceBus.CustomChecks](https://www.nuget.org/packages/NServiceBus.CustomChecks)
- [NServiceBus.Heartbeat](https://www.nuget.org/packages/NServiceBus.Heartbeat)
- [NServiceBus.SagaAudit](https://www.nuget.org/packages/NServiceBus.SagaAudit)
- [NServiceBus.ServicePlatform.Connector](https://www.nuget.org/packages/NServiceBus.ServicePlatform.Connector)
- [ServiceControl.Contracts](https://www.nuget.org/packages/ServiceControl.Contracts)

### Interoperability

These packages connect endpoints to systems running on a different transport, on another messaging technology, or outside the platform entirely:

- [NServiceBus.MessagingBridge](https://www.nuget.org/packages/NServiceBus.MessagingBridge)
- [NServiceBus.Gateway](https://www.nuget.org/packages/NServiceBus.Gateway)
- [NServiceBus.Gateway.Sql](https://www.nuget.org/packages/NServiceBus.Gateway.Sql)
- [NServiceBus.Gateway.RavenDB](https://www.nuget.org/packages/NServiceBus.Gateway.RavenDB)
- [NServiceBus.Envelope.CloudEvents](https://www.nuget.org/packages/NServiceBus.Envelope.CloudEvents) (experimental)

### Other

Optional features that extend endpoint behavior. Packages nested below another package extend it and are chosen to match the persistence or storage already in use:

- [NServiceBus.Testing](https://www.nuget.org/packages/NServiceBus.Testing)
- [NServiceBus.Encryption.MessageProperty](https://www.nuget.org/packages/NServiceBus.Encryption.MessageProperty)
- [NServiceBus.TransactionalSession](https://www.nuget.org/packages/NServiceBus.TransactionalSession)
  - [NServiceBus.Persistence.Sql.TransactionalSession](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.TransactionalSession)
  - [NServiceBus.Persistence.CosmosDB.TransactionalSession](https://www.nuget.org/packages/NServiceBus.Persistence.CosmosDB.TransactionalSession)
  - [NServiceBus.Persistence.DynamoDB.TransactionalSession](https://www.nuget.org/packages/NServiceBus.Persistence.DynamoDB.TransactionalSession)
  - [NServiceBus.Persistence.AzureTable.TransactionalSession](https://www.nuget.org/packages/NServiceBus.Persistence.AzureTable.TransactionalSession)
  - [NServiceBus.Storage.MongoDB.TransactionalSession](https://www.nuget.org/packages/NServiceBus.Storage.MongoDB.TransactionalSession)
  - [NServiceBus.RavenDB.TransactionalSession](https://www.nuget.org/packages/NServiceBus.RavenDB.TransactionalSession)
  - [NServiceBus.NHibernate.TransactionalSession](https://www.nuget.org/packages/NServiceBus.NHibernate.TransactionalSession)
- [NServiceBus.Callbacks](https://www.nuget.org/packages/NServiceBus.Callbacks)
  - [NServiceBus.Callbacks.Testing](https://www.nuget.org/packages/NServiceBus.Callbacks.Testing)
- [NServiceBus.ClaimCheck](https://www.nuget.org/packages/NServiceBus.ClaimCheck)
  - [NServiceBus.DataBus.AzureBlobStorage](https://www.nuget.org/packages/NServiceBus.DataBus.AzureBlobStorage)
  - [NServiceBus.DataBus.BinarySerializer](https://www.nuget.org/packages/NServiceBus.DataBus.BinarySerializer)
- [NServiceBus.UniformSession](https://www.nuget.org/packages/NServiceBus.UniformSession)
  - [NServiceBus.UniformSession.Testing](https://www.nuget.org/packages/NServiceBus.UniformSession.Testing)
- [NServiceBus.Wcf](https://www.nuget.org/packages/NServiceBus.Wcf)
- [Particular.Aspire.Hosting.ServicePlatform](https://www.nuget.org/packages/Particular.Aspire.Hosting.ServicePlatform)

### Tooling packages

These ship as .NET command-line tools for one-off migrations, rather than as packages referenced by an endpoint:

- [Particular.TimeoutMigration](https://www.nuget.org/packages/Particular.TimeoutMigration)
- [Particular.AzureTable.Export](https://www.nuget.org/packages/Particular.AzureTable.Export)
