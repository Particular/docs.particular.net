---
title: RabbitMQ Transport Upgrade Version 7 to 8
summary: Instructions on how to upgrade RabbitMQ Transport from version 7 to 8.
reviewed: 2022-05-05
component: Rabbit
related:
- transports/rabbitmq
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Timeout manager

The [timeout manager has been removed from NServiceBus 8](/nservicebus/upgrades/7to8/#timeout-manager-removed), so the `EnableTimeoutManager` backwards compatibility API has been removed from the transport. The [timeout migration tool](/nservicebus/tools/migrate-to-native-delivery.md) should be used to migrate any remaining timeout messages.

## Certificate connection string options removed

The `certPath` and `certPassphrase` connection string options have been removed. The `SetClientCertificate` API should be used instead.

## `requestedHeartbeat` connection string option removed

The `requestedHeartbeat` connection string option has been removed. The `SetHeartbeatInterval` API should be used instead.

## `retryDelay` connection string option removed

The `retryDelay` connection string option has been removed. The `SetNetworkRecoveryInterval` API should be used instead.

## `IRoutingTopology` `SetupSubscription` and `TeardownSubscription` changes

The `type` parameter of the `SetupSubscription` and `TeardownSubscription` methods of the `IRoutingTopology` interface has changed from `System.Type` to `NServiceBus.Unicast.Messages.MessageMetadata`. Custom routing topology implementations will need to be updated.