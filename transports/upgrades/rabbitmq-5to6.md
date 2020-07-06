---
title: RabbitMQ Transport Upgrade Version 5 to 6
summary: Instructions on how to upgrade RabbitMQ Transport Version 5 to 6.
reviewed: 2020-07-03
component: Rabbit
isUpgradeGuide: true
---

Version 6 is focused on supporting RabbitMQ.Client 6.1.0.

## .NET Framework

The minimal .NET Framework version is changed from 4.5.8 to 4.6.1 as this is the minimum version of RabbitMQ.Client 6.1.0.

## ReadOnlyMemory<byte> in `IRoutingTopology.RawSendInCaseOfFailure`

RabbitMQ.Client 6.x moves from `byte[]` to `ReadOnlyMemory<byte>` usage. This change is reflected in `IRoutingTopology.RawSendInCaseOfFailure` signature.

More information on usage [ReadOnlyMemory<T>](https://docs.microsoft.com/en-us/dotnet/standard/memory-and-spans/memory-t-usage-guidelines)

## `UseDurableExchangesAndQueues` is deprecated

The `UseDurableExchangesAndQueues` API has been deprecated in verson 6. The exchanges and queues are durable by default. Durable exchanges and queues can be turned off by calling `DisableDurableExchangesAndQueues`.
