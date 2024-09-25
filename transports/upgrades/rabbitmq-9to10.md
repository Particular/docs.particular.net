---
title: RabbitMQ Transport Upgrade Version 9 to 10
summary: Instructions on how to upgrade RabbitMQ Transport from version 9 to 10.
reviewed: 2024-09-25
component: Rabbit
related:
- transports/rabbitmq
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
---

## RabbitMQ Client v7 Upgrade

The transport now uses [RabbitMQ.Client v7.0.0](https://www.nuget.org/packages/RabbitMQ.Client/7.0.0), which exclusively supports an async API model. This change results in some breaking changes in the public API.

For details, see the [RabbitMQ client changelog](https://github.com/rabbitmq/rabbitmq-dotnet-client/releases/tag/v7.0.0).

### `IRoutingTopology` Updates

The following changes have been made to `IRoutingTopology`:

- All methods return a [ValueTask](https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/).
- The type of the `channel` parameter has been changed from `RabbitMQ.Client.IModel` to `RabbitMQ.Client.IChannel`.
- The type of the `properties` parameter has been changed from `RabbitMQ.Client.IBasicProperties` to `RabbitMQ.Client.BasicProperties`.
- All methods include a [CancellationToken](https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads) as the last parameter.
