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

## ReadOnlyMemory<byte>

RabbitMQ.Client 6.x is making use of `ReadOnlyMemory<byte>` where previously this was using `byte[]`. This changes leaks into `IRoutingTopology.RawSendInCaseOfFailure`.

More information on usage [ReadOnlyMemory<T>](https://docs.microsoft.com/en-us/dotnet/standard/memory-and-spans/memory-t-usage-guidelines)
