---
title: Azure Service Bus Transport Performance Tuning
summary: Azure Service Bus Performance Tuning
tags:
- Azure
- Cloud
- Configuration
---


### CPU vs IO bound processing

There are several things to consider:

 - `BatchSize`
 - `LockDuration`
 - `MaxDeliveryCount`

In scenarios where handlers are CPU intense and have very little IO, it is advised to lower number of threads to one and have a bigger `BatchSize`. `LockDuration` and `MaxDeliveryCount` might require an adjustment to match the batch size taking in account number of messages that end up in the dead letter queue.

In scenario where handlers are IO intense, it is advised to set number of threads ([`MaximumConcurrencyLevel`](/nservicebus/operations/tuning.md) in NServiceBus) to 12 threads per logical core and `BatchSize` to a number of messages that takes to process, taking into account possible/measured single message processing time and IO latency. Start with a small `BatchSize` and through adjustment and measurement bring it up, adjusting accordingly `LockDuration` and `MaxDeliveryCount`.