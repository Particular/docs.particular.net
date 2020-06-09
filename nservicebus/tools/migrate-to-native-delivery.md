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

It's important to note the definition of an endpoint to migrate in the context of the tool. The legacy [TimeoutManager](/nservicebus/messaging/timeout-manager.md) stored timeouts at the sending side and sent them out to the destination endpoint at delivery time. Pre-native delivery timeouts are owned by the TimeoutManager.
Native delivery timeouts however, are owned by the transport. Since the tool is migrating timeouts from the legacy implemetation, the endpoints that are being migrated are the sending endpoints.

Example:
Let's say you have a Sales endpoint that requested a timeout to be delivered to the Billing endpoint. The Billing endpoint is not requesting any timeouts.
Using the legacy [TimeoutManager](/nservicebus/messaging/timeout-manager.md), this means that the timeouts will be sent out by the Sales endpoint to the Billing endpoint when the delivery time is reached.
The tool will list the Sales endpoint as one of the options to migrate. Given that the Billing endpoint does not send timeouts, it won't be listed.
The tool will check that the Billing endpoint has the necessary infrastructure in place to handle native delivery.

The tool supports the use of the `--cutofftime`. This is the starting point in time from which timeouts become elegible to migrate, based on the time to deliver set in the timeout.
There are two main reasons to make use of this:
- SLA compliance: In case there are many timeouts in the storage to migrate, it might take some time for the migration to complete. Since the timeouts are first hidden from the legacy [TimeoutManager](/nservicebus/messaging/timeout-manager.md) and then migrated, this might result in some timeouts being delivered later than their original delivery time in case of large migrations.
- Phasing the migration: In case of big loads of timeouts to migrate, it might be interesting to run a phased migration based on the original delivery time of the timeouts. This can be realised by setting the `--cutofftime` to a far point in the future, and decrease it each run.
When the tool starts, it will first analyse the endpoints that have timeouts to migrate, and will generate an overview of the number of timeouts for that endpoint.


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
- `--cutofftime`: The time from which to start migrating timeouts, it makes sense to start migrating timeouts that will expire at least one day in the future. The format in which to specify the `cutofftime` is `yyyy-MM-dd HH:mm:ss:ffffff Z`. The migration tool will convert the specified `cutofftime` to UTC time.
- `--endpoint`: The endpoint to migrate.
- `--allendpoints`: Indicates to migrate all endpoints in one run
`--endpoint` and `--allendpoints` arguments are mutually exclusive, either one is required.

Depending on the persistence, there are additional parameters required in order to run the migration:

For RavenDB:
- `--serverUrl`: The RavenDB server URL
- `--databaseName`: The database name where timeouts to migrate are stored
- `--prefix`: The prefix used for storage of timeouts. The default value is "TimeoutDatas"
- `--ravenVersion`: The allowed values are "3.5" and "4"

For SQL:
- `--source`: The connection string to the database
- `--dialect`: The SQL dialect used to access the database

#### Running a migration

Migrating from RavenDB persistence

```
migrate-timeouts ravendb
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
migrate-timeouts sqlp
                        -t|--target <targetConnectionString>
                        -c|--cutofftime <cutofftime>
                        --source <source>
                        --dialect <sqlDialect>
                        [--endpoint] <endpointName>
                        [--allendpoints]
                        [-a|--abort]
```

NOTE: The listed endpoints will be the in the escaped form that is used to prefix the endpoints timeout table

Aborting a migration

```
migrate-timeouts <repeat previous parameters>
                        [-a|--abort]
```

## How the tool works

The migration tool will perform a few health checks:
 - verify it's able to connect to the storage
 - verify it's able to connect to the target transport
 - verify that the necessary topology is in place and the target transport supports native delayed delivery
 - list all the endpoints for which the tool can detect timeouts
 - calculate the amount of timeouts to migrate per endpoint
 - validate if there are timeouts the tool is [unable to migrate](migrate-to-native-delivery.md#limitations)

Once this information has been reviewed, the migration process can be started.
Even though the tool supports migrating all endpoints connected to the persistence at once, it is highly suggested to migrate endpoint by endpoint, especially for critical endpoints. Even when selecting the `--allendpoints` option, the tool will execute an endpoint-by-endpoint migration behind the scenes.

## Limitations

As documented in the [RabbitMQ transport](/transports/rabbitmq/delayed-delivery.md), the maximum delay value of a timeout is 8,5 years. If the migration tool encounters any timeouts that have delivery time set beyond that, it won't be possible to migrate that endpoint.

If the tool presents endpoints that are not part of the system when running the `--analyze` option, it might be that an endpoint was renamed at some point.
Any timeouts that were stored for that endpoint, might already be late in delivery and should be handled seperate from the migration tool since the tool has no way to detect where to migrate them to.

## What if something goes wrong

Verify that you provided the correct arguments in order to connect to the storage and the destination transport.

If the migration started but stopped or failed along the way, the migration tool can recover and continue where it left off. To resume an interrupted migration the tool must be run with the same arguments.

To run the tool with different arguments any in-progress or pending migration needs to be aborted using the `--abort` option. Any timeouts that have been fully migrated at that point will not be rollbacked, they will have already been delivered to the target transport and may even have been already consumed by the destination endpoint. Timeouts that were scheduled to migrate will be rollbacked and made available again to the legacy TimeoutManager.
