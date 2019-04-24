---
title: Transitioning to the SQL Server Transport
summary: Demonstrates the required changes to switch your POC from the Learning Transport to the SQL Server Transport
reviewed: 2019-04-24
component: SqlTransport
tags:
 - Transport
related:
 - transports/sql
 - samples/sqltransport/simple
---

The Learning transport is not a production-ready transport, but is intended for learning the NServiceBus API and creating demos/proof-of-concepts. A number of changes to endpoint configuration are required to transition an endpoint from the Learning Transport to the SQL Server Transport.


## Install the NuGet Package

The [NServiceBus.SqlServer](https://www.nuget.org/packages/NServiceBus.SqlServer/) must be installed. This can be accomplished using the [NuGet Package Manager UI](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui) or run this command from the [NuGet Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console):

```
   Install-Package NServiceBus.SqlServer
```


## Change the Endpoint Configuration

To use the MSMQ Transport first update the `UseTransport` call:

```c#
-   endpointConfiguration.UseTransport<LearningTransport>();
+   endpointConfiguration.UseTransport<SqlServerTransport>();
```

### Set a Connection String

The SQL Server Transport requires a connection string be specified:

snippet: Usage


### Enable Installers

The endpoint must enable installers to allow the MSMQ Transport to create the necessary [queues](https://msdn.microsoft.com/en-us/library/ms705002.aspx) for your endpoint.

snippet: Installers


### Configure a Persistence

This is because the SQL Server Transport, unlike the Learning Transport, does not natively support Publish/Subscribe and instead uses [message-driven Pub/Sub](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based), so the message subscription information must be stored.

snippet: SqlPersistenceUsageSqlServer

[SQL Persistence using Microsoft SQS Server](/persistence/sql/) is shown here as an example, however use the [selecting a persister](/persistence/selecting) guide to help determine the appropriate persistence for the endpoint.


include: registerpublishers
