---
title: SQL Server transport in Azure SQL
summary: Overview of the SQL Server transport performance in Azure SQL
component: SqlTransport
reviewed: 2020-06-12
versions: '[6.0.1,)'
---

NOTE: Prior to NServiceBus.SqlServer [version 6.0.1](https://github.com/Particular/NServiceBus.SqlServer/releases/tag/6.0.1), the SQL Server transport used clustered indexes based on an identity column (sequence number). This structure was prone to high contention that caused severe issues in high-throughput scenarios, especially on Azure SQL. The [index migration guide](/transports/upgrades/sqlserver-non-clustered-idx.md) describes the details of upgrading to the new table schema. 

Using SQL Server with Azure SQL allows building messaging systems that provide exactly-once message processing guarantees. The same instance of Azure SQL is used both as an application data store and as messaging infrastructure. 

This article discusses throughput characteristics of the SQL Server transport in Azure SQL. Numbers presented here are *rough estimates* of what can be expected when running on Azure SQL. All measurements were made on a [Mananaged Instance](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-managed-instance) database running in [vCore General Purpose](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-service-tiers-vcore?tabs=azure-portal) service tier.

### Testing methodology

A small set of stress tests has been done to estimate the maximum throughput on a single queue. It yielded the following results:

| vCore | msg/s|
|-|-|
|8|600|
|16|800|


While useful, these results are not representative of most real-life scenarios. In most deployments there are at least tens or even hundreds of endpoints, each processing messages with much smaller throughput.

A set of load tests were designed to measure the CPU usage while processing messages coming at a constant rate. Messages were processed by a chain of 15 endpoints. Each endpoint in the chain was forwarding messages to the next one.

Here are the CPU usage values:

* 2 vCPU

| msgs/s | resource % |
|-|-|
|25 | 30% IO, 35% CPU|
|50 | 65% IO, 65% CPU|
|60 | 80% IO, 80% CPU|

* 4 vCPU

| msgs/s | resource % |
|-|-|
|25 | 15% IO, 15% CPU|
|50 | 35% IO, 35% CPU|
|75 | 50% IO, 60% CPU|
|90 | 60% IO, 85% CPU| 

* 8 vCPU

| msgs/s | resource % |
|-|-|
|25 | 8% IO, 8% CPU|
|50 | 17% IO, 17% CPU|
|75 | 25% IO, 30% CPU|
|100 | 32% IO, 50% CPU|
|125 | 40% IO, 80% CPU|

* 16 vCPU

| msgs/s | resource % |
|-|-|
|25 | 9% IO, 4% CPU|
|50 | 17% IO, 9% CPU|
|75 | 25% IO, 16% CPU|
|100 | 33% IO, 25% CPU|
|125 | 40-50% IO, 35-45% CPU|

### Discussion

The total system throughput (for 15 endpoints in the test) for different per-endpoint throughputs are:

| single endpoint msg/s | total msg/s |
|-|-|
|25 | 375 |
|50 | 750 |
|60 | 900 |
|75 | 1125 |
|100 | 1500 |
|125 | 1875 |

In these scenarios the database is used both as a transport and as a data store for the application state, so it is recommended to assume that the CPU usage of the transport should not exceed 35% on average. That means that for a system consisting of 15 endpoints, the recommended vCPU count as a function of total throughput is:

| total msg/s | vCores |
|-|-|
|0-375 | 2  |
|375-750 | 4  |
|750-1125 | 8  |
|1125-1500 | 16  |

NOTE: The exact values for a production system may vary but the table above should provide a basis for estimations. It is worth noting that the SQL Server transport scales better with number of endpoints than with throughput of each endpoint. That means that higher total throughput can be achieved with higher number of endpoints and lower per-endpoint throughput. In an extreme case, a 16 vCPU database can handle only around 800 messages with a single queue and well over 2000 messages when running with 15 queues.

When designing Software-as-a-Service systems, it is recommended to use separate databases and endpoint sets for groups of tenants rather than for vertical slices of a business flow. With this approach each database would support relatively high number (whole business flow end-to-end) of low-throughput endpoints (a portion of customer base) as opposed to low number of high-throughput endpoints.

When using multiple instances of SQL Azure consider connecting them with [NServiceBus.Router](/nservicebus/router/). In order to ensure exactly-once message processing semantics, consider [elastic transactions](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-elastic-transactions-overview) in the message router.
