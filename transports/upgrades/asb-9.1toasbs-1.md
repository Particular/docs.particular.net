---
title: Azure Service Bus Transport Legacy Upgrade Version 9.1 to Azure Service Bus 1.0
summary: Tips when upgrading Azure Service Bus Legacy transport to Azure Service Bus.
reviewed: 2020-02-26
component: ASBS
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 ---


## Migrating from Azure Service Bus (Legacy) Transport to the Azure Service Bus Transport

Endpoints using the Azure Service Bus Transport can be run side-by-side with endpoints using the Legacy Azure Service Bus Transport as long as the [backwards compatibility](/transports/azure-service-bus/compatibility) requirements are met.

## API Differences

There are differences between the API for the Legacy Azure Service Bus Transport and the Azure Service Bus Transports.

### Max lock duration

It is not possible to change the default message lock duration or the auto-renewal for locks.

Azure Service Bus uses a default of 5 minutes for the message lock duration and it does not auto-renew this. It is not possible to change these values as they have been chosen for enhanced reliability and to suppor the most common use-cases. This involves specifying a lock duration of 5 minutes. One of the reasons we chose 5 mins LockDuration is due to the fact there was a lot of confusion about ASB's default (30 seconds), Prefetch, and AutoLockRenewal.

The most common use case for NServiceBus endpoints is to process messages without the need to manage for lock expiration. Therefore the default values for LockDuration and Prefetch in the Azure Service Bus Transport have been selected to ensure no messages lose their locks when processing is longer than 30 seconds. If processing is successful, the message will be ack'd and completed. In case the message fails processing, it will go through recoverability. Only when there's an abnormal shutdown behaviour (e.g. such as an endpoint crashing) would take up-to 5 minutes for the message to re-appear.

### Forward dead lettered messages to

It is not possible to specify a queue to forward dead lettered messages to.

### Default message time to live

It is not possible to specify a default time to live at a transport level and these should be configured by using one of the methods of [discarding old messages](/nservicebus/messaging/discard-old-messages).

### Enable dead lettering on message expiration

It is not possible to forward expired messages automatically to the dead letter queue.

### Auto delete on idle

It is not possible to automaticall delete queues that have not been used for a period of time.

### Enable batched operations

It is not possible to configure server side batched operations.

### Requires duplicate detection

It is not possible to configure whether the broker should perform native duplicate detection or not.

### Support ordering

Ordering is not supported.