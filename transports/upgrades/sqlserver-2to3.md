---
title: SQL Server Transport Upgrade Version 2 to 3
summary: Instructions on how to upgrade the SQL Server transport from version 2 to 3
reviewed: 2020-02-20
component: SqlTransport
related:
 - nservicebus/upgrades/5to6
redirects:
 - nservicebus/upgrades/sqlserver-2to3
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Namespace changes

The primary namespace is `NServiceBus`. Advanced APIs have been moved to `NServiceBus.Transport.SqlServer`. A `using NServiceBus` directive should be sufficient to find all basic options. A `using NServiceBus.Transport.SqlServer` statement is needed to access these configuration options.


### Transactions

The native transaction support has been split into two levels: `ReceiveOnly` and `SendAtomicWithReceive`. Both are supported. `SendAtomicWithReceive` is equivalent to disabling distributed transactions in NServiceBus version 5.

```csharp
// For SQL Transport version 3.x
var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

// For SQL Transport version 2.x
var transactions = busConfiguration.Transactions();
transactions.DisableDistributedTransactions();
```

As shown in the above snippet, transaction settings are now handled in the transport level configuration.

For more details and examples refer to the [transaction configuration API](/nservicebus/upgrades/5to6/transaction-configuration.md) and [transaction support](/transports/transactions.md) pages.


### Connection factory

The custom connection factory method is now expected to be `async` and no parameters are passed to it by the framework:

```csharp
// For SQL Transport version 3.x
var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
transport.UseCustomSqlConnectionFactory(
    sqlConnectionFactory: async () =>
    {
        var connection = new SqlConnection("SomeConnectionString");
        try
        {
            await connection.OpenAsync()
                .ConfigureAwait(false);

            // perform custom operations

            return connection;
        }
        catch
        {
            connection.Dispose();
            throw;
        }
    });

// For SQL Transport version 2.x
var transport = busConfiguration.UseTransport<SqlServerTransport>();
transport.UseCustomSqlConnectionFactory(
    connectionString =>
    {
        var connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();

            // custom operations

            return connection;
        }
        catch
        {
            connection.Dispose();
            throw;
        }
    });
```


### Accessing transport connection

Accessing transport connection can be done in version 2 of the transport by injecting `SqlServerStorageContext` into a handler. This way the handler can access the data residing in the same database as the queue tables within the message receive transaction managed by the transport.

In version 3, this API has been removed. The same goal can be achieved in version 3 by using ambient transaction mode.

```csharp
var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
transport.Transactions(TransportTransactionMode.TransactionScope);
```

The handler must open a connection to access the data, but assuming both handler and the transport are configured to use the same connection string, there is no DTC escalation. SQL Server 2008 and later can detect that both connections target the same resource and merges the two transactions into a single lightweight transaction.

NOTE: The above statement is does not apply when using the NHibernate persistence. NHibernate persistence opens its own connection within the context of the ambient transaction. See [access business data](/persistence/nhibernate/accessing-data.md) in context of processing a message with NHibernate persistence.


### Multi-schema support

The configuration API for [multi-schema support](/transports/sql/deployment-options.md#multi-schema) has now changed. The `QueueSchema` parameter is no longer supported in the config file and the code configuration API.

The schema for the configured endpoint can be specified using `DefaultSchema` method:

```csharp
// For SQL Transport version 3.x
var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
transport.DefaultSchema("myschema");

// For SQL Transport version 2.x
var transport = busConfiguration.UseTransport<SqlServerTransport>();
transport.DefaultSchema("myschema");
```

or by defining a custom schema per endpoint or queue:

```csharp
var transport = busConfiguration.UseTransport<SqlServerTransport>();
transport.UseSpecificConnectionInformation(
    EndpointConnectionInfo.For("sales")
        .UseSchema("sender"),
    EndpointConnectionInfo.For("error")
        .UseSchema("control")
);
```

This enables configuring custom schema both for local endpoint as well as for other endpoints that the configured endpoint communicates with. When using configuration file to specify schemas for other endpoints, their schemas should be placed in the `MessageEndpointMappings` section and follow `endpoint-name@schema-name` convention:

```xml
<UnicastBusConfig>
  <MessageEndpointMappings>
    <add Assembly="Billing.Contract"
         Endpoint="billing@schema_name"/>
    <add Assembly="Sales.Contract"
         Endpoint="sales@another_schema_name"/>
  </MessageEndpointMappings>
</UnicastBusConfig>
```


### Multi-instance support

The configuration API for multi-instance support has changed. Multiple connection strings must be provided by a connection factory method passed to `EnableLegacyMultiInstanceMode` method.

Note that `EnableLegacyMultiInstanceMode` method replaces both pull and push modes from version 2.x.

```csharp
// For SQL Transport version 3.x
var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
transport.EnableLegacyMultiInstanceMode(
    sqlConnectionFactory: async address =>
    {
        string connectionString;
        if (address == "RemoteEndpoint")
        {
            connectionString = "SomeConnectionString";
        }
        else
        {
            connectionString = "SomeOtherConnectionString";
        }
        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync()
            .ConfigureAwait(false);
        return connection;
    });

// For SQL Transport version 2.x
var transport = busConfiguration.UseTransport<SqlServerTransport>();
// Option 1
transport
    .UseSpecificConnectionInformation(
        EndpointConnectionInfo.For("RemoteEndpoint")
            .UseConnectionString("SomeConnectionString"),
        EndpointConnectionInfo.For("AnotherEndpoint")
            .UseConnectionString("SomeOtherConnectionString"));

// Option 2
transport
    .UseSpecificConnectionInformation(
        connectionInformationProvider: x =>
        {
            if (x == "RemoteEndpoint")
            {
                var connection = ConnectionInfo.Create();
                return connection.UseConnectionString("SomeConnectionString");
            }
            if (x == "AnotherEndpoint")
            {
                var connection = ConnectionInfo.Create();
                return connection.UseConnectionString("SomeOtherConnectionString");
            }
            return null;
        });
```

Note that `multi-instance` mode has been deprecated and is not recommended for new projects.


#### Multi-instance support and outbox

Version 3 does not support outbox in `multi-instance` mode.


### Circuit breaker

The method `PauseAfterReceiveFailure(TimeSpan)` is no longer supported. In version 3, the pause value is hard-coded at one second.


### Indexes

Queue tables created by version 2.2.1 or lower require manual creation of a non-clustered index on the `[Expires]` column. The following SQL statement can be used to create the missing index:

```sql
CREATE NONCLUSTERED INDEX [Index_Expires]
ON [schema].[queuename]
(
	[Expires] ASC
)
INCLUDE
(
	[Id],
	[RowVersion]
)
```

A warning will be logged when a missing index is detected.
