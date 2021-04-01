---
title: SQL Server Transport Upgrade Version 6 to 7
summary: Migration instructions on how to migrate the SQL Server transport from version 6 to version 7
reviewed: 2021-04-01
component: SqlTransport
related:
- transports/sql
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

## Configuring SQL Server transport

To use the SQL Server transport for NServiceBus, create a new instance `SqlServerTransport` and pass it to the `EndpointConfiguration.UseTransport` method.

Instead of

```csharp
var transport = endpointConfiguration.UseTransport<SqlServerTransprot>();
transport.ConnectionString(connectionString);
```

use:

```csharp
var transport = new SqlServerTransport(connectionString);
endpointConfiguration.UseTransport(transport);
```

## Configuration options

The SQL Server transport configuration options have moved to the `SqlServerTransport` class. See the following table for further information:

| Version 6 configuration option | Version 7 configuration option |
| --- | --- |
| CreateMessageBodyComputedColumn | CreateMessageBodyComputedColumn |
| DefaultSchema | DefaultSchema |
| NativeDelayedDelivery | DelayedDelivery|
| PurgeExpiredMessagesOnStartup | ExpiredMessagesPurger.PurgeOnStartup |
| WithPeekDealy | QueuePeeker |
| SubscriptionSettings | Subscriptions |
| TimeToWaitBeforeTriggeringCircuitBreaker | TimeToWaitBeforeTriggeringCircuitBreaker |
| TransactionScopeOptions | TransactionScope |
| UseSchemaForEndpoint| UseSchemaForEndpoint on `RoutingSettings`|
| UseCatalogForEndpoint | UseCatalogForEndpoint on `RoutingSettings` |
| UseSchemaForQueue | SchemaAndCatalog.UseSchemaForQueue |
| UseCatalogForQueue | SchemaAndCatalog.UseCatalogForQueue |
| UseCustomSqlConnectionFactory | constructor argument |

## Timeout manager

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout manager backward-compatibility mode obsolete. If backward-compatibility mode is enabled, these APIs must be removed.

## WithPeekDelay replaced by QueuePeeker

In version 6 of the transport, the message peek delay can be defined using the `WithPeekDelay` configuration option. The configuration setting has moved to a more generic `QueuePeeker` transport property that allows configuration of other parameters related to message peeking.

## UseScheamForEndpoint and UseCatalogForEndpoint do not affect the local endpoint

In version 6 of the transport, `UseSchemaForEndpoint` and `UseCatalogForEndpoint` affected the local endpoint address. In version 7, this is no longer the case. Calling one of the methods for local endpoint throws and exception and should be replaced with `UseSchemaForQueue` and `UseCatalogForQueue` calls.

snippet: 6to7-main-endpoint-custom-schema-and-catalog