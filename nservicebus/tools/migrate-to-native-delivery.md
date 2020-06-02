---
title: Migrating to native delivery
summary: An overview of the tool supporting migrating from timeout manager to native delivery
reviewed: 2020-05-29
---

In v7 [native delayed delivery](/nservicebus/messaging/delayed-delivery.md) was introduced across most transports that are supported today.
Hybrid mode was made available for endpoints, which means that the timeouts that were already registered in the system, would still be consumed by the legacy [TimeoutManager] (/nservicebus/messaging/timeout-manager.md) and the new delayed deliveries  would flow through the native implementation.

In v8, support for the hybrid mode is removed. Most of the timeouts that were still registered through the legacy timeout manager might have been consumed by now, but there might be scenario's in the system in which there are remain timeouts waiting to expire.
For those use cases, there is a .NET Core global tool that enables migration of those timeouts to the native delayed delivery infrastructure.

The tool supports live-migration so there's no need to shut down the endpoints before running the tool. The tool will hide the timeouts it will migrate from the legacy TimeoutManager to eliminate duplicate deliveries in the system.
The tool provides a preview command in order to gather an overview of the endpoints and timeouts to migrate in the system.

## Supported persistence mechanisms

The current version of the tool supports the following persistence mechanisms:
- [SQL](/persistence/sql/) using SQL Server under the hood
- [RavenDB](/persistence/ravendb) versions 3 and 4

## Supported transports

The current version of the tool supports the following transports:
- [RabbitMQ](/transports/rabbitmq/)

## How to install

```
dotnet tool install migrate-timeouts -g --source
```

To verify if the tool was installed correctly, run:

```
dotnet tool list -g
```

It should be listed there.

## Using the tool

Preview the migration

```
dotnet migrate-timeouts preview
                        -t|--target <targetConnectionString>
                        -c|--cutofftime <cutofftime>
                        [--endpoint <endpointName>]
                        [--allendpoints]
                        [-a|--abort]

```

Running a migration

```
dotnet migrate-timeouts -t|--target <targetConnectionString>
                        -c|--cutofftime <cutofftime>
                        [--endpoint <endpointName>]
                        [--allendpoints]
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
