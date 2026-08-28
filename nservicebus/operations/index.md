---
title: Operations
summary: Operations Table of Contents
reviewed: 2026-08-28
---

Operational tasks include the following categories:

* Data migrations
* Upgrading endpoints
* Deployments
* Diagnostics
* Scripting activities (using the low level/native APIs to help perform any of the above tasks)

Some of these tasks apply to every endpoint. The rest are best reviewed from the perspective of the selected transport and persistence, which are listed below when they have dedicated operational guidance.

## Endpoints

* [Installers](/nservicebus/operations/installers.md)
* [Startup diagnostics](/nservicebus/hosting/startup-diagnostics.md)
* [OpenTelemetry](/nservicebus/operations/opentelemetry.md)
* [Tuning message throughput performance and concurrency](/nservicebus/operations/tuning.md)
* [Decommissioning endpoints](/nservicebus/endpoints/decommissioning-endpoints.md)

## Upgrades

* [NServiceBus upgrade guides](/nservicebus/upgrades/)
* [Transport upgrade guides](/transports/upgrades/)
* [Persistence upgrade guides](/persistence/upgrades/)

## Transports

### [Azure Service Bus Transport](/transports/azure-service-bus)

* [Scripting](/transports/azure-service-bus/operational-scripting.md)
* [Queue-scoped permissions](/transports/azure-service-bus/queue-scoped-permissions.md)

### [Azure Storage Queues Transport](/transports/azure-storage-queues/)

* [Performance Tuning](/transports/azure-storage-queues/performance-tuning.md)
* [Use multiple accounts for scale out](/transports/azure-storage-queues/multi-storageaccount-support.md)
* [Scripting](/transports/azure-storage-queues/operations-scripting.md)
* [Troubleshooting](/transports/azure-storage-queues/troubleshooting.md)

### [Amazon SQS Transport](/transports/sqs/)

* [Performance Tuning](/transports/sqs/performance-tuning.md)
* [Scripting](/transports/sqs/operations-scripting.md)
* [Troubleshooting](/transports/sqs/troubleshooting.md)

### [RabbitMQ Transport](/transports/rabbitmq/)

* [Routing topology](/transports/rabbitmq/routing-topology.md)
* [Scripting](/transports/rabbitmq/operations-scripting.md)

### [SQL Server Transport](/transports/sql/)

* [Deployment options](/transports/sql/deployment-options.md)
* [Scripting](/transports/sql/operations-scripting.md)
* [Azure SQL failover and connection pooling](/transports/sql/azure-sql-failover.md)
* [Troubleshooting](/transports/sql/troubleshooting.md)

### [PostgreSQL Transport](/transports/postgresql/)

* [Deployment considerations](/transports/postgresql/#deployment-considerations)

### [IBM MQ Transport](/transports/ibmmq/)

* [Scripting](/transports/ibmmq/operations-scripting.md)
* [Observability](/transports/ibmmq/observability.md)

### [MSMQ Transport](/transports/msmq/)

* [Scripting](/transports/msmq/operations-scripting.md)
* [Management using PowerShell](/transports/msmq/management-using-powershell.md)
* [Dead letter queues](/transports/msmq/dead-letter-queues.md)
* [Viewing message content](/transports/msmq/viewing-message-content-in-msmq.md)
* [Troubleshooting](/transports/msmq/troubleshooting.md)

## Persistences

### [SQL Persistence](/persistence/sql/)

* [Installer workflow](/persistence/sql/installer-workflow.md)
* [Scripting](/persistence/sql/operational-scripting.md)
* [Controlling script generation](/persistence/sql/controlling-script-generation.md)
* [MS SQL Server Scripts](/persistence/sql/sqlserver-scripts.md)
* [MySql Scripts](/persistence/sql/mysql-scripts.md)
* [Oracle Scripts](/persistence/sql/oracle-scripts.md)
* [PostgreSQL Scripts](/persistence/sql/postgresql-scripts.md)
* [Troubleshooting](/persistence/sql/troubleshooting.md)

### [Cosmos DB Persistence](/persistence/cosmosdb/)

* [Capacity planning using request units (RU)](/persistence/cosmosdb/#capacity-planning-using-request-units-ru)
* [Provisioned throughput rate-limiting](/persistence/cosmosdb/#provisioned-throughput-rate-limiting)

### [DynamoDB Persistence](/persistence/dynamodb/)

* [Table creation](/persistence/dynamodb/table-creation.md)
* [Capacity planning](/persistence/dynamodb/capacity-planning.md)

### [Azure Table Persistence](/persistence/azure-table/)

* [Performance Tuning](/persistence/azure-table/performance-tuning.md)
* [Capacity planning](/persistence/azure-table/capacity-planning.md)
* [Scripting](/persistence/azure-table/scripting.md)

### [NHibernate Persistence](/persistence/nhibernate/)

* [Scripting](/persistence/nhibernate/scripting.md)

### [RavenDB Persistence](/persistence/ravendb/)

* [Scripting](/persistence/ravendb/operations-scripting.md)
* [Installing RavenDB](/persistence/ravendb/installation.md)
* [Cluster configuration with multiple nodes](/persistence/ravendb/cluster-configuration.md)
