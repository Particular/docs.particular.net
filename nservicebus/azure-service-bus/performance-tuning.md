---
title: Azure Service Bus Transport Performance Tuning
tags:
- Azure
- Cloud
- Configuration
---


### CPU vs IO bound processing

The following settings are used in order to tune performance in Azure Service Bus:

 - `MaximumConcurrencyLevel`
 - `BatchSize`
 - `LockDuration`
 - `MaxDeliveryCount`

In scenarios where handlers are CPU intense and have very little IO, it is advised to lower the number of threads to one and increase the `BatchSize`. `LockDuration` and `MaxDeliveryCount` might require an adjustment to match the batch size, taking into account the number of messages that end up in the dead letter queue.

In scenario where handlers are IO intense, it is advised to set the number of threads  to 12 threads per logical core using `MaximumConcurrencyLevel` setting, and set the `BatchSize` to a number of messages that takes to process. Take into account the expected (or measured) processing time and IO latency of a single message. Start with a small `BatchSize` and through adjustment and measurement gradually increase it, while adjusting accordingly `LockDuration` and `MaxDeliveryCount`.

For more information on those settings, refer to the [Tuning endpoint message processing](/nservicebus/operations/tuning.md), [ASB Batching](/nservicebus/azure-service-bus/batching.md), [ASB Message lock renewal](/nservicebus/azure-service-bus/message-lock-renewal.md) and [ASB Retry behavior](/nservicebus/azure-service-bus/retries.md) articles. 