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

## Migrating from Azure Service Bus (Legacy) to Azure Service Bus Transport

Endpoints using the Azure Service Bus Transport can be run side-by-side with endpoints using the legacy Azure Service Bus transport as long as the [backwards compatibility](/transports/azure-service-bus/compatibility.md) requirements are met.

## API Differences

There are differences between the API for the legacy Azure Service Bus and the Azure Service Bus transports.

### Max lock duration

Azure Service Bus set message lock duration (`MaxLockDuration`) to a maximum of 5 minutes by default. This value has been chosen for enhanced reliability and to support the most common use-cases. One of the reasons to set lock duration to the maximum is to avoid the need to reconfigure the setting when the default time of 30 seconds is not sufficient for processing and which would cause `LockLostException` exceptions. 

The most common use case for NServiceBus endpoints is to process messages without the need to manage for lock expiration. Therefore the default values for LockDuration and Prefetch in the Azure Service Bus transport have been selected to ensure no messages lose their locks when processing is longer than 30 seconds. If processing is successful, the message will be acknowledged and completed. In case the message fails to process, it will go through recoverability. Only when there's an abnormal shutdown behaviour (e.g. such as an endpoint crashing) would take up to 5 minutes for the message to re-appear.

Q: is it possible to change the default message lock duration for an endpoint?
A: yes. The transport is no longer modifying any existing queues (or other entities). Lock duration modification should be done directly on the queue using the [Azure portal](https://portal.azure.com/), [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/servicebus/queue?view=azure-cli-latest#az-servicebus-queue-update), or a 3rd part tool.

Q: would a lock duration set to the maximum time have a negative impact on a message that fails to process in less than 5 minutes?
A: no. The transport will release the message. If it fails to do so, the message will be unlocked automatically by the broker and become available for re-processing.

Q: do I still need to consider the size of the prefetch with the lock duration set to maximum?
A: yes. While the new transport has been build with the most considerable defaults, handlers are still custom code and if take too long, even the optimal defaults can become sub-optimal.

### Forward dead-lettered messages to

To specify dead-letter messages forwarding use [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/servicebus/queue?view=azure-cli-latest#az-servicebus-queue-update) or a 3rd part tool.


### Default message time to live

It is not possible to specify a default time

To specify message time to live, use the [Azure portal](https://portal.azure.com/), [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/servicebus/queue?view=azure-cli-latest#az-servicebus-queue-update), or a 3rd part tool. 

Alternatively,  message time to live can be configured by using one of the methods of [discarding old messages](/nservicebus/messaging/discard-old-messages.md).

### Enable dead lettering on message expiration

To enable dead-lettering on message expiration, use [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/servicebus/queue?view=azure-cli-latest#az-servicebus-queue-update) or a 3rd part tool.


### Auto-delete on idle

To enable auto-delete on idle, use [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/servicebus/queue?view=azure-cli-latest#az-servicebus-queue-update) or a 3rd part tool.


### Enable batched operations

To enable server-side batched operations, use [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/servicebus/queue?view=azure-cli-latest#az-servicebus-queue-update) or a 3rd part tool.


### Requires duplicate detection


To enable duplicate detection, use [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/servicebus/queue?view=azure-cli-latest#az-servicebus-queue-update) or a 3rd part tool.


### Support ordering

Ordering is not supported.
