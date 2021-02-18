---
title: Migrating from timeout manager to native delivery
summary: An overview of the tool supporting migrating from timeout manager to native delivery
reviewed: 2020-06-22
---

The timeout migration tool is designed to help system administrators migrate existing timeouts from the legacy [timeout manager](/nservicebus/messaging/timeout-manager.md) storage to the [native delayed delivery](/nservicebus/messaging/delayed-delivery.md) infrastructure of the currently used transport.

[Native delayed delivery](/nservicebus/messaging/delayed-delivery.md) was introduced in NServiceBus version 7 across most supported transports and a hybrid mode was made available which is enabled by default. In hybrid mode, endpoints consume timeouts that were registered in the system using the legacy [timeout manager](/nservicebus/messaging/timeout-manager.md) while new delayed messages flow through the native implementation.

NOTE: Be sure to use the `preview` option of the tool to make sure that the transport supports native delayed delivery before attempting a migration.

Most of the timeouts that were registered through the legacy timeout manager might have been consumed by now. But there might be scenarios in which there are timeouts waiting to expire, and those are stored in the timeout storage. For those use cases, the timeout migration tool allows migrating timeouts to the native delayed delivery infrastructure so that the storage can be decommissioned.

The tool supports live migration in most cases so there's no need to shut down the endpoints before running the tool. The tool will hide the timeouts to be migrated from the legacy timeout manager to eliminate duplicate deliveries in the system.

WARN: Migrating a high number of timeouts on RavenDB may require endpoints to be shutdown to improve performance. Run the `preview` command to find out if this is the case.

It's important to note the definition of an endpoint to migrate in the context of the tool. The legacy [timeout manager](/nservicebus/messaging/timeout-manager.md) stored timeouts at the sending side and sent them out to the destination endpoint at delivery time. This means that the endpoint names listed by the tool is for the endpoints **sending** the delayed message and not the destination.

Example:

- There is a Sales endpoint that requested a timeout to be delivered to the Billing endpoint. The Billing endpoint is not requesting timeouts.
- With the legacy [timeout manager](/nservicebus/messaging/timeout-manager.md), the timeouts will be sent out by the Sales endpoint to the Billing endpoint when the delivery time is reached.
- The tool will list the Sales endpoint as one of the options to migrate. Given that the Billing endpoint does not send timeouts, it won't be listed.
- The tool will check that the Billing endpoint has the necessary infrastructure in place to handle native delivery.

The tool also supports a `--cutoffTime` parameter. This is the starting point in time from which timeouts become eligible to migrate, based on the time to deliver set in the timeout.

There are two reasons to use the `--cutoffTime` parameter:

- SLA compliance: If there are many timeouts in the storage to migrate, it might take some time for the migration to complete. Since the timeouts are first hidden from the legacy [timeout manager](/nservicebus/messaging/timeout-manager.md) and then migrated, this might result in some timeouts being delivered later than their original delivery time in case of large migrations.
- Phasing the migration: In case of a high number of timeouts to migrate, it may be prudent to run a phased migration based on the original delivery time of the timeouts. This can be achieved by setting the `--cutoffTime` to a far point in the future, and decrease it each run.

## Supported persisters

The current version of the tool supports the following persisters:

- [SQL persistence](/persistence/sql/) using the SQL Server implementation
- [NHibernate persistence](/persistence/nhibernate/) using the Sql Server and Oracle implementation
- [RavenDB persistence](/persistence/ravendb) versions 3.5.x and 4.x of the RavenDB database server
- [Azure Storage persistence](/persistence/azure-table) version 2.4.x compatible timeouts stored in Azure Storage tables

## Supported transports

The tool supports the following transports:

- [RabbitMQ transport](/transports/rabbitmq/)
- [SQL transport](/transports/sql)
- [Azure Storage Queues transport](/transports/azure-storage-queues/)

## Before using the tool

Even though the tool doesn't delete any timeout information when doing the migration, it is recomended to follow industry standards related to modifying the database. Create a backup of the production database and run the migration on a test environment before running it in production.

## How to install

`dotnet tool install Particular.TimeoutMigration -g`

To verify if the tool was installed correctly:

`dotnet tool list -g`

Verify the tool is listed among the available installed tools.

## Tool options

The migration tool provides a `preview`, `migrate` and `abort` command.

### Preview

To get a preview of endpoints and their status use the `preview` command and specify the required source and target with the corresponding options.

```
migrate-timeouts preview source
                        <source-specific-options>
                        target
                        <target-specific-options>
```

### Migrate

To run a migration for selected endpoint(s) use the `migrate` command with the following parameters.

- `--target`: The connection string of the target transport
- `--endpoint`(Optional): The endpoint to migrate.
- `--allEndpoints`(Optional): Whether to migrate all endpoints in one run
- `--cutoffTime`(Optional): The time from which to start migrating timeouts. In general, it makes sense to start migrating timeouts that will expire at least one day in the future. The format in which to specify the `cutoffTime` is `yyyy-MM-dd HH:mm:ss`. The migration tool will convert the specified `cutoffTime` to UTC time.

NOTE: `--endpoint` and `--allEndpoints` arguments are mutually exclusive. One of them must be provided.

```
migrate-timeouts migrate source
                        <source-specific-options>
                        target
                        <target-specific-options>
                        [-c|--cutoffTime <cutoffTime>]
                        [--endpoint] <endpointName>
                        [--allendpoints]
```

### Abort

To abort an ongoing migration use the `abort` command. Abort must also specify the previously selected target including the target specific arguments.

```
migrate-timeouts abort source
                        <source-specific-options>
                        target
                        <target-specific-options>
```

### Source options

```
migrate-timeouts command ravendb|sqlp|nhb
                        <source-specific-options>
                        target
                        <target-specific-options>
```

For RavenDB (`ravendb`) persistence:

- `--serverUrl`: The RavenDB server URL
- `--databaseName`: The database name where timeouts to migrate are stored
- `--ravenVersion`: Allowed values are "3.5" and "4"
- `--prefix`(optional): The prefix used for storage of timeouts. The default value is "TimeoutDatas"
- `--forceUseIndex`(Optional): Required when migrating large amounts of timeouts. Requires all endpoints using the database to be turned off so as not to modify the timeout data. This option will only be used during migration.

For SQL (`sqlp`) persistence:

- `--source`: The connection string to the database
- `--dialect`: The SQL dialect used to access the database. Supported dialects: `MsSqlServer`

NOTE: The listed endpoints will be in the escaped format that is used to prefix the endpoints timeout table.

For NHibernate (`nhb`) persistence:

- `--source`: The connection string to the database
- `--dialect`: The SQL dialect used to access the database. Supported dialects: `MsSqlServer` and `Oracle`

NOTE: The listed endpoints will be in the escaped format that is used to prefix the endpoints timeout table.

For Azure Storage (`asp`) persistence:

- `--source`: The connection string to the Azure Storage Account
- `--endpoint`(Mandatory): The endpoint to migrate
- `--timeoutTableName`: The timeout table name to migrate timeouts from
- `--partitionKeyScope`: The partition key scope format to be used (must follow the pattern of starting with year, month, and day)
- `--containerName`: The container name to be used to download timeout data from

### Target options

```
migrate-timeouts command source
                        <source-specific-options>
                        rabbitmq|sqlt|asq
                        <target-specific-options>
```

For RabbitMQ (`rabbitmq`) transport:

- `--target`: The RabbitMQ connection string

For SqlServer (`sqlt`) transport:

- `--target`: The SQL Server connection string, including the catalog
- `--schema`: The schema in which to the timeout tables are stored, defaults to `dbo`

For Azure Storage Queues (`asq`) transport:

- `--target`: The Azure Storage connection string to be used
- `--delayedtablename`: The delayed delivery table name to use. This is only required when the name of the delayed delivery table has been overridden from the default. It is not possible to migrate all endpoints when specifying this option.

## How the tool works

The migration tool will first perform a few health checks:

- verify it's able to connect to the storage
- verify it's able to connect to the target transport
- verify that the target transport supports native delayed delivery
- check that necessary infrastructure for native delays is setup for all delayed message destinations found
- list all the endpoints for which the tool can detect timeouts
- calculate the amount of timeouts to migrate per endpoint
- validate if there are timeouts the tool is [unable to migrate](migrate-to-native-delivery.md#limitations)

Even though the tool supports migrating all endpoints connected to the persister at once, it is strongly recommended to migrate endpoint by endpoint, especially for critical endpoints. Even when selecting the `--allEndpoints` option, the tool will execute an endpoint-by-endpoint migration behind the scenes.

## Cleanup

The tool will not delete any timeouts or storage artifacts in order to prevent data loss. This section describes how to clean up archived timeouts and remove storage artifacts that are no longer used.

WARN: This is a destructive operation and should only be performed once a successful migration has been verified.

### RavenDB persistence

- Ensure that [RabbitMQ compatibility mode](/transports/rabbitmq/delayed-delivery.md#backwards-compatibility) is turned off
- Delete all documents in the `TimeoutDatas` documents that have an `OwningTimeoutManager` starting with `__migrated__`

### Sql persistence

Use `SELECT * FROM TimeoutsMigration_State` to list all performed migrations. For all the successful ones do the following:

- Make sure that [RabbitMQ compatibility mode](/transports/rabbitmq/delayed-delivery.md#backwards-compatibility) is turned off
- Delete the empty `{EndpointName}_TimeoutData` table
- Delete the migration table named `TimeoutData_migration_{MigrationRunId}`, where `MigrationRunId` is taken from the output of the `TimeoutsMigration_State` query (this will free up the disk space used by the timeouts)

### NHibernate persistence

 - Delete the `StagedTimeoutEntity` table. This table contains copies of migrated timeouts.
 - Delete the `MigrationsEntity` table. Each row in this table represents a previously performed migration.

### Azure Storage persistence

When all the timeouts in a given timeout table have been migrated, the timeout table may be deleted. It is also possible to delete only migrated entities from a timeout table by deleting all rows with `OwningTimeoutManager` starting with `__hidden__`.

The `timeoutsmigration` table is left in the state of the last migration that was run in case it is required for troubleshooting. The table is cleaned automatically during every migration run. To reduce storage costs, after all migrations are done it is advisable to delete:

- The migration table: `timeoutsmigration` 
- The timeout tool state table: `timeoutsmigrationtoolstate`

## Limitations

### RabbitMQ transport

As documented in the [RabbitMQ transport](/transports/rabbitmq/delayed-delivery.md), the maximum delay value of a timeout is 8.5 years. If the migration tool encounters any timeouts that have delivery time set beyond that, it will not migrate that endpoint's timeouts.

If the tool presents endpoints that are not part of the system when running the `preview` command, it's possible that an endpoint was renamed at some point. Any timeouts that were stored for that endpoint, might already be late in delivery and should be handled separately from the migration tool since the tool has no way to detect where to migrate them to.

### RavenDB persistence

The tool requires that timeout documents be discoverable with a known prefix. The prefix is passed to the tool using the `--prefix` parameter. The default is `TimeoutDatas` if a value is not provided. If the system being migrated is using custom ID generation strategies when persisting timeout documents, a prefix may not be applicable.

Scanning timeouts without a well-known prefix is currently not supported.

### Azure Storage persistence

Due to restrictions of Azure Storage Tables it is not possible to list all endpoints or migrate multiple endpoints. Therefore the `--endpoint` option must be specified for all commands.

### Azure Storage Queues transport

When migrating timeouts to the [ASQ transport](/transports/azure-storage-queues/), the table in which delayed messages are stored is determined by convention. The same convention is applied in the tool. However, it's possible to [override delayed messages table name in the endpoint configuration](/transports/azure-storage-queues/delayed-delivery.md). If this option is used, the tool is unable to migrate all endpoints as the convention can't be applied to derive the table name for the delayed messages. Therefore, the tool will guard against this scenario and require that the `--endpoint` option be used when the `--delayedtablename` option is specified.

## Troubleshooting

If the migration started but stopped or failed along the way, the migration tool can recover and continue where it left off. To resume an interrupted migration the tool must be run with the same arguments.

To run the tool with different arguments, any in-progress migrations first need to be aborted using the `abort` command. Any timeouts that have been fully migrated at that point will not be restored since they have already been migrated to the native timeout infrastructure. Timeouts that were scheduled to migrate will be made available again to the legacy timeout manager.

### Logging

Turn on verbose logging using the `--verbose` option.

### RavenDB persistence

Use Raven Studio to check the state of ongoing and previous migrations by filtering documents using the prefix `TimeoutMigrationTool`. Completed migrations are named `TimeoutMigrationTool/MigrationRun-{endpoint}-{completed-time}` while any ongoing migration are present as `TimeoutMigrationTool/State`. The contents of the documents contains metadata about the migration such as the time started, time completed, used cutoff time etc.

### Sql persistence

The history and migrated data is always kept in the database.

To list the history and status of migrations execute:

`SELECT * FROM TimeoutsMigration_State`

To list the status of timeouts for a previous/in-progress run, take the `MigrationRunId` from the results of the previous query and execute:

`SELECT * FROM TimeoutData_migration_{MigrationRunId}`

The results include the batch number and its status: `0=Pending`, `1=Staged`, or `2=Completed`.

### NHibernate persistence

The history and migrated data is always kept in the database.

To list the history and status of migrations:

`SELECT * FROM MigrationsEntity`

To list all the timeouts that were staged for migration:

`SELECT * FROM StagedTimeoutEntity` 

The results include the batch number and its status: `0=Pending`, `1=Staged`, or `2=Completed`.

### Azure Storage persistence

To list the history and status of migrations open the `timeoutsmigrationtoolstate` Azure Table inside the Storage Explorer.

The migrated timeouts and their status are contained in the `timeoutsmigration` Azure Table. This includes the batch number its status: `0=Pending`, `1=Staged`, or `2=Completed`.

To list all timeouts that were transferred from the endpoints timeout table to the `timeoutsmigration` table, execute the following query and adjust the partition key cut-off time accordingly. To avoid returning additional timeout data, scope the partition key to the cutoff time plus one hundred years.

```
PartitionKey ge '2021-02-09' and PartitionKey le '2121-02-09' and OwningTimeoutManager ge '__hidden__' and PartitionKey le '__hidden_`'
```

To list all not yet migrated timeouts for a given endpoint use the following query:

```
PartitionKey ge '2021-02-09' and PartitionKey le '2121-02-09' and OwningTimeoutManager eq 'endpointname'
```

Adjust the partition key cut-off time and the endpoint name accordingly. To avoid returning additional timeout data, scope the partition key to the cutoff time plus one hundred years.
