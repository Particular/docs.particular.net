---
title: RabbitMQ Transport Upgrade Version 5 to 6
summary: Instructions on how to upgrade RabbitMQ Transport from version 5 to 6.
reviewed: 2022-05-06
component: Rabbit
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

Version 6 of the RabbitMQ transport is focused on supporting RabbitMQ.Client 6.1.0.

## .NET Framework

The minimum .NET Framework version is changed from 4.5.2 to 4.6.1 as this is the minimum version supported by RabbitMQ.Client 6.1.0.

## `ReadOnlyMemory<byte>` in `IRoutingTopology.RawSendInCaseOfFailure`

This affects the [custom routing topology](/transports/rabbitmq/routing-topology.md#custom-routing-topology). RabbitMQ.Client 6.x uses `ReadOnlyMemory<byte>` where it previously used `byte[]`. This change is reflected in the `IRoutingTopology.RawSendInCaseOfFailure` signature.

More information on using [ReadOnlyMemory<T>](https://docs.microsoft.com/en-us/dotnet/standard/memory-and-spans/memory-t-usage-guidelines)

## `UseDurableExchangesAndQueues` is deprecated

The `UseDurableExchangesAndQueues` API has been deprecated in version 6. The exchanges and queues are durable by default. Non-durable exchanges and queues can be used by calling `DisableDurableExchangesAndQueues`.

## `UsePublisherConfirms` is deprecated

The `UsePublisherConfirms` API has been deprecated in version 6. Publisher confirms are always enabled and there no longer is a way to disable publisher confirms.

## `UseRoutingTopology` has been renamed

The `UseRoutingTopology` API has been renamed to `UseCustomRoutingTopology` to make it clearer that the API is used when a custom routing topology has been implemented.

## `SetClientCertificates` has been renamed

The `SetClientCertificates` API has been renamed to `SetClientCertificate` to indicate that only a single certificate can be set.
