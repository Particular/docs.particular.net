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

The `certPath` and `certPassphrase` connection string options have been removed. The [`SetClientCertificate`](/transports/rabbitmq/connection-settings.md#transport-layer-security-support-client-authentication) API should be used instead.

## `requestedHeartbeat` connection string option removed

The `requestedHeartbeat` connection string option has been removed. The [`SetHeartbeatInterval`](/transports/rabbitmq/connection-settings.md#controlling-behavior-when-the-broker-connection-is-lost-heartbeat-interval) API should be used instead.

## `retryDelay` connection string option removed

The `retryDelay` connection string option has been removed. The [`SetNetworkRecoveryInterval`](/transports/rabbitmq/connection-settings.md#network-recovery-interval) API should be used instead.

## Transport no longer claims to support `TransportTransactionMode.None`

The transport does not have any functional difference between `TransportTransactionMode.ReceiveOnly` and `TransportTransactionMode.None` modes, but there has been no way to indicate this before NServiceBus 8. Now that NServiceBus 8 has enabled this, the transport now only supports `TransportTransactionMode.ReceiveOnly`.

## `IRoutingTopology` `SetupSubscription` and `TeardownSubscription` changes

The `type` parameter of the `SetupSubscription` and `TeardownSubscription` methods of the `IRoutingTopology` interface has changed from `System.Type` to `NServiceBus.Unicast.Messages.MessageMetadata`. [Custom routing topology](/transports/rabbitmq/routing-topology.md#custom-routing-topology) implementations will need to be updated.