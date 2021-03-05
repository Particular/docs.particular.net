---
title: Azure Service Bus Transport Legacy 9.1 to Azure Service Bus 1.0
summary: Azure Service Bus legacy to Azure Service Bus transport upgrade guide.
reviewed: 2020-02-26
component: ASBS
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 ---

## Migrating from Azure Service Bus (legacy) to Azure Service Bus transport

Endpoints using the Azure Service Bus transport can be run side-by-side with endpoints using the legacy Azure Service Bus transport as long as the [backward compatibility](/transports/azure-service-bus/compatibility.md) requirements are met.

## Infrastructure differences

The Azure Service Bus transport uses ports 5671, 5672, and 443 to communicate with Azure servers. Configure firewall rules to allow outbound traffic through these ports.

The legacy Azure Service Bus transport uses ports 9350 to 9354 to communicate with Azure servers. These ports are not used by the latest (non-legacy) Azure Service Bus transport. Any previously configured firewall rules to allow outbound traffic through these ports can be decommissioned.

## API Differences

There are differences between the API for the legacy Azure Service Bus and the Azure Service Bus transports.

### Max lock duration

Azure Service Bus sets message lock duration (`MaxLockDuration`) to a maximum of 5 minutes by default. This value has been chosen for enhanced reliability and to support the most common use cases. One reason to set lock duration to the maximum is to avoid the need to reconfigure the setting when the default time of 30 seconds is not sufficient for processing and which would cause `LockLostException` exceptions. 

The most common use case for NServiceBus endpoints is to process messages without the need to manage for lock expiration. Therefore the default values for `LockDuration` and `Prefetch` in the Azure Service Bus transport have been selected to ensure no messages lose their locks when processing takes longer than 30 seconds. If processing is successful, the message will be acknowledged and completed. If the message fails to process, it will go through [the recoverability mechanism](/nservicebus/recoverability/). Only when there's an abnormal shutdown behaviour (e.g. such as an endpoint crashing) would it take up to 5 minutes for the message to re-appear.

**Is it possible to change the default message lock duration for an endpoint?**

Yes. The transport is no longer modifying existing queues (or other entities). Lock duration modification should be done directly on the queue using the [Azure portal](https://portal.azure.com/), [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/servicebus/queue?view=azure-cli-latest#az-servicebus-queue-update), or a third party tool.

**Would a lock duration set to the maximum time have a negative impact on a message that fails to process in less than 5 minutes?**

No. The transport will release the message. If it fails to do so, the message will be unlocked automatically by the broker and become available for re-processing.

**Do I still need to consider the size of the prefetch with the lock duration set to maximum?**

Yes. While the new transport has been built with reasonable defaults, if a handler takes too long, even the defaults can become sub-optimal.

### Max Delivery Count

`MaxDeliveryCount` no longer has the number of Processing Attempts assigned to it, but instead is set to `int.MavValue`. This is because the number of deliveries is not actually related to the number of processing attempts.

### Azure CLI options

The following settings can be configured via the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/servicebus/queue?view=azure-cli-latest#az-servicebus-queue-update) or a third party tool. These options were configured using the Endpoint Configuration API in the legacy Azure Service Bus transport.

* Dead-lettered messages forwarding
* Default message time to live. Alternatively, message time to live can be configured using one of the methods of [discarding old messages](/nservicebus/messaging/discard-old-messages.md).
* Enable dead-lettering on message expiration
* Auto-delete on idle
* Enable batched operations
* Requires duplicate detection

### Removed options

Any configuration options not already explictly covered that was previously accessed via the `Queues()`, `Topics()`, or `Subscriptions` [settings](/transports/azure-service-bus/legacy/configuration/full.md#controlling-entities) have been removed.

The configuration options for controlling connectivity as well as the physical addressing logic have also been removed and replaced with optimal defaults for most use cases. These settings include:

* Controlling connectivity
  * `NumberOfClientsPerEntity`
  * `TransportType`
  * `BrokeredMessageBodyType`
  * `MessagingFactories`
  * `MessageReceivers`
  * `MessageSenders`
* Physical addressing logic
  * `UseNamespaceNamesInsteadOfConnectionStrings`
  * `Sanitization` - the new transport offers [shorteners](/transports/azure-service-bus/configuration.md#entity-creation) for subscriptions and rules to control entity name length
  * `Individualization`
  * `NamespacePartitioning`
  * `Composition`

The [message lock renewal](/transports/azure-service-bus/legacy/message-lock-renewal.md) feature was removed. A custom pipeline behavior may be used instead. See the message lock renewal [sample](/samples/azure-service-bus-netstandard/lock-renewal) for more details.

NOTE: Some legacy transport features, such as namespace partitioning for high availability, were removed in favor of the native broker features. Customers are advised to evaluate the Service Bus Premium tier to take advantage of those native features.
