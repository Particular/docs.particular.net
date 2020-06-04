---
title: Migrating to native delivery
summary: An overview of the tool supporting migrating from timeout manager to native delivery
reviewed: 2020-05-29
---

The timeout migration tool is designed to help system administrators to migrate existing timeouts from the legacy [TimeoutManager](/nservicebus/messaging/timeout-manager.md) storage to the [native delayed delivery](/nservicebus/messaging/delayed-delivery.md) infrastructure of the currently used transport.

NOTE: Make sure to check that the transport in use supports native delayed delivery before performing any migration attempt.
In v7 [native delayed delivery](/nservicebus/messaging/delayed-delivery.md) was introduced across most supported transports.
Hybrid mode was made available, and enabled by default, for endpoints. When running in hybrid mode, endpoints consume timeouts that were already registered in the system using the legacy [TimeoutManager](/nservicebus/messaging/timeout-manager.md) and new delayed messages flow through the native implementation.

Most of the timeouts that were registered through the legacy timeout manager might have been consumed by now. There might be scenarios in which there are timeouts waiting to expire, and those are stored in the timeout storage.
For those use cases, the timeout migration .NET Core global tool enables migrating timeouts to the native delayed delivery infrastructure.

The tool supports live-migration so there's no need to shut down the endpoints before running the tool. The tool will hide the timeouts it will migrate from the legacy TimeoutManager to eliminate duplicate deliveries in the system.
The tool provides a preview command in order to gather an overview of the endpoints and timeouts to migrate in the system.

## Supported persistence mechanisms

The current version of the tool supports the following persistence mechanisms:
- [SQL Persistence](/persistence/sql/) using the SQL Server implementation
- [RavenDB](/persistence/ravendb) versions 3.5.x and 4.x of the RavenDB database server

## Supported transports

The tool supports the following transports:
- [RabbitMQ](/transports/rabbitmq/)

## How to install

```
dotnet tool install migrate-timeouts -g
```

To verify if the tool was installed correctly, run:

```
dotnet tool list -g
```

Verify the tool is listed among the available installed tools.

## Using the tool

The migration tool expects a few parameters in order to successfully migrate the timeouts.

These parameters are required independent of the persistence used:
- `--target`: The connection string of the target transport
```--cutofftime```:     The time from which to start migrating timeouts, it makes sense to start migrating timeouts that will expire at least one day in the future.
```--endpoint```:       The endpoint to migrate.
```--allendpoints```:   Indicates to migrate all endpoints in one run
Even though parameters --endpoint and --allendpoints are option, one of them is required by the tool.

Depending on the persistence, there are a few additonal parameters needed in order to run the migration:

For RavenDB:
```--serverUrl```:      The server url for the persistence
```--databaseName```:   The name of the database in which timeouts are stored
```--prefix```:         The prefix used for storage of timeouts, the default is "TimeoutDatas"
```--ravenVersion```:   The supported versions for RavenDB are 3.5 and 4

For SQL:
```--source```:         The connection string to the database
```--tablename```:      The name of the table in which timeouts are stored
```--dialect```:        The sql dialect used to access the database

#### Preview the migration

The tool can be run in preview mode in order to get an overview of the endpoints available, and the timeouts within those endpoint to migrate.
It's highly suggested to run in preview mode first and verify that the results match the expected timeouts to migrate.

RavenDB example:

```
dotnet migrate-timeouts preview ravendb
                        -t|--target <targetConnectionString>
                        -c|--cutofftime <cutofftime>
                        --serverUrl <serverUrl>
                        --databaseName <databaseName>
                        [--prefix] <prefix>
                        [--ravenVersion] <ravenVersion>
                        [--endpoint] <endpointName>
                        [--allendpoints]
```

SQL example:

```
dotnet migrate-timeouts preview sqlp
                        -t|--target <targetConnectionString>
                        -c|--cutofftime <cutofftime>
                        --source <source>
                        --tablename <tablename>
                        --dialect <sqlDialect>
                        [--endpoint] <endpointName>
                        [--allendpoints]
```

#### Running a migration

Migrating from RavenDB persistence

```
dotnet migrate-timeouts ravendb
                        -t|--target <targetConnectionString>
                        -c|--cutofftime <cutofftime>
                        --serverUrl <serverUrl>
                        --databaseName <databaseName>
                        [--prefix] <prefix>
                        [--ravenVersion] <ravenVersion>
                        [--endpoint] <endpointName>
                        [--allendpoints]
                        [-a|--abort]

```

Migrating from Sql persistence

```
dotnet migrate-timeouts preview sqlp
                        -t|--target <targetConnectionString>
                        -c|--cutofftime <cutofftime>
                        --source <source>
                        --tablename <tablename>
                        --dialect <sqlDialect>
                        [--endpoint] <endpointName>
                        [--allendpoints]
                        [-a|--abort]

```

Aborting a migration

```
dotnet migrate-timeouts <repeat previous parameters>
                        [-a|--abort]

```

## How the tool works

Before actually migrating any timeouts, it's highly recommended to use the preview option.
This option will perform the following actions:
 - check that the tool can connect to the storage
 - check that the tool can connect to the target transport
 - verify that the necessary topology is in place in the target transport to support native delayed delivery
 - list all the endpoints for which the tool can detect timeouts
 - calculate the amount of timeouts to migrate per endpoint
 - validate if there are timeouts the tool is [unable to migrate](migrate-to-native-delivery.md#limitations)

Once this information has been reviewed, the migration process can be initialized.
Even though the tool supports migrating all endpoints connected to the persistence at once, it is highly suggested to migrate endpoint by endpoint, especially for critical endpoints. Even when selecting the -allendpoints options, the tool will conduct an endpoint-by-endpoint migration behind the scenes.

## Limitations

As documented in the [RabbitMQ transport](/transports/rabbitmq/delayed-delivery.md), the maximum delay value of a timeout is 8,5 years. If the migration tool encounters any timeouts that have delivery times set beyond that, it won't be possible to migrate that endpoint.

If the tool presents endpoints that are not part of the system when running the --analyze option, it might be that an endpoint was renamed at some point.
Any timeouts that were stored for that endpoint, might already be late in delivery and should be handled seperate from the migration tool since the tool has no way to detect where to migrate them to.

## What if something goes wrong

If the migration is stopped or fails for some reason, the tool is able to recover and continue where it left off last time, considering that the tool is run with the same parameters. In order to run the tool with different parameters after initialising the migration process, the running migration process needs to be aborted using the --abort option. Any timeouts that have been fully migrated at that point will not be rollbacked, they will have already be delivered to the target transport and may even have been already delivered to the destination endpoint. Timeouts that were scheduled to migrate will be rollbacked and reappear to the legacy TimeoutManager.
