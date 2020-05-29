---
title: Migrating to native delivery
summary: An overview of the tool supporting migrating from timeout manager to native delivery
reviewed: 2020-05-29
---

In v7 we introduced [native delayed delivery](/nservicebus/messaging/delayed-delivery.md) across most transports we support today.
You could setup your endpoints to run in hybrid mode, which means that the timeouts that were already registered in your system, would still be consumed by the legacy [TimeoutManager] (/nservicebus/messaging/timeout-manager.md) and the new delayed deliveries that were registered by the system, would flow through the native implementation.

In v8 we're removing support for the hybrid mode. Most of the timeouts that were still registered through the legacy timeout manager might have been consumed by now, but we realise that there might be scenario's in your system in which there are still timeouts waiting in your system that have yet to expire.
For those use cases, we have created a .net tool that will enable you to migrate those timeouts to the native delayed delivery infrastructure.

The tool supports live-migration so there's no need to shut down your endpoints before running the tool. The tool will make the timeouts that will be migrated invisble to the legacy TimeoutManager to eliminate duplicate deliveries in your system.
We support an analyze command in order to gather an overview of the endpoints and timeouts to migrate in your system.

## Supported persistence mechanisms

The current version of the tool supports the following persistence mechanisms:
- [SQL](/persistence/sql/) using SQL Server under the hood
- [RavenDB](/persistence/RavenDB) versions 3 and 4

## Supported transports

The current version of the tool supports the following transports:
- [RabbitMQ](/transports/rabbitmq/)

## How to install

```
dotnet tool install migrate-timeouts -g
```

If you want to verify if the tool was installed correctly, run:

```
dotnet tool list -g
```

You should see it listed there.

## Using the tool

```
dotnet migrate-timeouts -t|--target <targetConnectionString>
                        -c|--cutofftime <cutofftime>
                        [--endpoint <endpointName>]
                        [--allendpoints]
                        [-a|--abort]

```

## How the tool works

Before actually migrating any timeouts, we recommend to use the analyze option that the tool supports.
This option will perform the following actions:
 - check that the we can connect to the storage
 - check that we can connect ot the target transport
 - verify that the necessary topology is in place in the target transport to support native delayed delivery
 - list all the endpoints for which we can detect timeouts
 - calculate the amount of timeouts to migrate per endpoint
 - validate if there are timeouts we are [unable to migrate](#Limitations)

Once you have reviewed this data, you are ready to migrate the timeouts.
We suggest to migrate endpoint by endpoint, especially for critical endpoints. Even when selecting the -allendpoints options, we will conduct a endpoint-by-endpoint migration behind the scenes.


## Limitations

As documented in the [RabbitMQ transport](/transports/rabbitmq/delayed-delivery.md), the maximum delay value of a timeout is 8,5 years. If the migration tool encounters any timeouts that have delivery set beyond that time, it won't be possible to migrate that endpoint.

If the tool presents you with endpoints that you don't recognise when running the --analyze option, it might be that an endpoint was renamed at some point.
Any timeouts that were stored for that endpoint, might already be late in delivery and should be handled seperate from the migration tool since the tool has no way to detect where to migrate them to.

## Community-maintained transports

There are several community-maintained transports which can be found in the full list of [extensions](/components#transports).
