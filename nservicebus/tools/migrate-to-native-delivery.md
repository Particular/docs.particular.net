---
title: Migrating from timeout manager to native delivery
summary: An overview of the tool supporting migrating from timeout manager to native delivery
reviewed: 2020-05-29
---

The timeout migration tool is designed to help system administrators migrate existing timeouts from the legacy [Timeout Manager](/nservicebus/messaging/timeout-manager.md) storage to the [native delayed delivery](/nservicebus/messaging/delayed-delivery.md) infrastructure of the currently used transport.

NOTE: Make sure to use the `preview` option of the tool to make sure that the transport supports native delayed delivery before performing any migration attempt.

In v7 [native delayed delivery](/nservicebus/messaging/delayed-delivery.md) was introduced across most supported transports and a hybrid mode was made available, and enabled by default. When running in hybrid mode, endpoints consume timeouts that were already registered in the system using the legacy [Timeout Manager](/nservicebus/messaging/timeout-manager.md) and new delayed messages flow through the native implementation.

Most of the timeouts that were registered through the legacy timeout manager might have been consumed by now. But there might be scenarios in which there are timeouts waiting to expire, and those are stored in the timeout storage.
For those use cases, the timeout migration tool enables migrating timeouts to the native delayed delivery infrastructure so that the storage can be decommissioned.

The tool supports live-migration so there's no need to shut down the endpoints before running the tool. The tool will hide the timeouts it will migrate from the legacy Timeout Manager to eliminate duplicate deliveries in the system.

It's important to note the definition of an endpoint to migrate in the context of the tool. The legacy [Timeout Manager](/nservicebus/messaging/timeout-manager.md) stored timeouts at the sending side and sent them out to the destination endpoint at delivery time. This means that the endpoint names listed by the tool is for the endpoints **sending** the delayed message and not the destination.

Example:

Let's say there is a Sales endpoint that requested a timeout to be delivered to the Billing endpoint. The Billing endpoint is not requesting any timeouts.
Using the legacy [Timeout Manager](/nservicebus/messaging/timeout-manager.md), this means that the timeouts will be sent out by the Sales endpoint to the Billing endpoint when the delivery time is reached.
The tool will list the Sales endpoint as one of the options to migrate. Given that the Billing endpoint does not send timeouts, it won't be listed.
The tool will check that the Billing endpoint has the necessary infrastructure in place to handle native delivery.

The tool optionally supports the use of the `--cutoffTime`. This is the starting point in time from which timeouts become eligible to migrate, based on the time to deliver set in the timeout.
There are two main reasons to make use of this:

- SLA compliance: In case there are many timeouts in the storage to migrate, it might take some time for the migration to complete. Since the timeouts are first hidden from the legacy [Timeout Manager](/nservicebus/messaging/timeout-manager.md) and then migrated, this might result in some timeouts being delivered later than their original delivery time in case of large migrations.
- Phasing the migration: In case of big loads of timeouts to migrate, it might be interesting to run a phased migration based on the original delivery time of the timeouts. This can be achieved by setting the `--cutoffTime` to a far point in the future, and decrease it each run.

## Supported legacy persistence

The current version of the tool supports the following persisters:

- [SQL Persistence](/persistence/sql/) using the SQL Server implementation
- [RavenDB](/persistence/ravendb) versions 3.5.x and 4.x of the RavenDB database server

## Supported transports

The tool supports the following transports:

- [RabbitMQ](/transports/rabbitmq/)

## How to install

`dotnet tool install migrate-timeouts -g`

To verify if the tool was installed correctly, run:

`dotnet tool list -g`

Verify the tool is listed among the available installed tools.

## Using the tool

The migration tool provides a `preview`, `migrate` and `abort` command.

Depending on the persistence, there are additional parameters required in order to run the tool:

For RavenDB:

- `--serverUrl`: The RavenDB server URL
- `--databaseName`: The database name where timeouts to migrate are stored
- `--prefix`: The prefix used for storage of timeouts. The default value is "TimeoutDatas"
- `--ravenVersion`: The allowed values are "3.5" and "4"

For SQL:

- `--source`: The connection string to the database
- `--dialect`: The SQL dialect used to access the database. Supported dialects: `MsSqlServer`

### Preview

To get a preview of endpoints and their status use the `preview` command with the following extra parameters.

- `--target`: The connection string of the target transport used to validate destinations

**RavenDB**

```
migrate-timeouts ravendb preview
                        -t|--target <targetConnectionString>
                        --serverUrl <serverUrl>
                        --databaseName <databaseName>
                        [--prefix] <prefix>
                        [--ravenVersion] <ravenVersion>
```

**Sql Persistence**

```
migrate-timeouts sqlp preview
                        -t|--target <targetConnectionString>
                        --source <source>
                        --dialect <sqlDialect>
```

NOTE: The listed endpoints will be the in the escaped form that is used to prefix the endpoints timeout table

### Running a migration

To run a migration for selected endpoint(s) use the `migrate` command with the following parameters.

- `--target`: The connection string of the target transport
- `--endpoint`(Optional): The endpoint to migrate.
- `--allEndpoints`(Optional): Indicates to migrate all endpoints in one run
- `--cutoffTime`(Optional): The time from which to start migrating timeouts, it makes sense to start migrating timeouts that will expire at least one day in the future. The format in which to specify the `cutoffTime` is `yyyy-MM-dd HH:mm:ss`. The migration tool will convert the specified `cutoffTime` to UTC time.

NOTE: `--endpoint` and `--allEndpoints` arguments are mutually exclusive, either one is required.

**RavenDB**

```
migrate-timeouts ravendb migrate
                        -t|--target <targetConnectionString>
                        --serverUrl <serverUrl>
                        --databaseName <databaseName>
                        [--prefix] <prefix>
                        [--ravenVersion] <ravenVersion>
                        [--endpoint] <endpointName>
                        [-c|--cutofftime <cutofftime>]
                        [--allendpoints]
```

**Sql Persistence**

```
migrate-timeouts sqlp migrate
                        -t|--target <targetConnectionString>
                        --source <source>
                        --dialect <sqlDialect>
                        [-c|--cutofftime <cutofftime>]
                        [--endpoint] <endpointName>
                        [--allendpoints]
```

NOTE: The listed endpoints will be the in the escaped form that is used to prefix the endpoints timeout table

### Aborting a migration

To abort an ongoing migration use the `abort` command.

**RavenDB**

```
migrate-timeouts ravendb abort
                        --serverUrl <serverUrl>
                        --databaseName <databaseName>
                        [--prefix] <prefix>
                        [--ravenVersion] <ravenVersion>
```

**Sql Persistence**

```
migrate-timeouts sqlp abort
                        --source <source>
                        --dialect <sqlDialect>
```

## How the tool works

The migration tool will perform a few health checks:

- verify it's able to connect to the storage
- verify it's able to connect to the target transport
- verify that the target transport supports native delayed delivery
- check that necessary infrastructure for native delays is setup for all delayed message destinations found
- list all the endpoints for which the tool can detect timeouts
- calculate the amount of timeouts to migrate per endpoint
- validate if there are timeouts the tool is [unable to migrate](migrate-to-native-delivery.md#limitations)

Once this information has been reviewed, the migration process can be started.
Even though the tool supports migrating all endpoints connected to the persistence at once, it is highly suggested to migrate endpoint by endpoint, especially for critical endpoints. Even when selecting the `--allendpoints` option, the tool will execute an endpoint-by-endpoint migration behind the scenes.

## Limitations

As documented in the [RabbitMQ transport](/transports/rabbitmq/delayed-delivery.md), the maximum delay value of a timeout is 8,5 years. If the migration tool encounters any timeouts that have delivery time set beyond that, it won't be possible to migrate that endpoint.

If the tool presents endpoints that are not part of the system when running the `preview` command, it might be that an endpoint was renamed at some point.
Any timeouts that were stored for that endpoint, might already be late in delivery and should be handled separate from the migration tool since the tool has no way to detect where to migrate them to.

## Troubleshooting

If the migration started but stopped or failed along the way, the migration tool can recover and continue where it left off. To resume an interrupted migration the tool must be run with the same arguments.

To run the tool with different arguments any in-progress migration needs to be aborted using the `abort` command. Any timeouts that have been fully migrated at that point will not be restored since they already have been delivered to the native timeout infrastructure. Timeouts that were scheduled to migrate will be made available again to the legacy Timeout Manager.

### Logging

Turn on verbose logging using the `--verbose` option.

### RavenDB

TBD

### Sql Persistence

The history and migrated data is always kept in the database so nothing can get lost.

To list the history and status of migrations execute:

`SELECT * FROM TimeoutsMigration_State`

To list the status of timeouts for an a previous/in-progress run take the `MigrationRunId` from the query about and execute:

`SELECT * FROM TimeoutData_migration_{MigrationRunId}`

This will show all the timeouts and to which batch they belong and also that status of that batch, `0=Pending`, `1=Staged` and `2=Completed`.
