---
title: Performance Tuning
summary: How the Azure Storage Queues transport receives messages in parallel by default, and how to tune it for throughput and storage costs.
component: ASQ
isLearningPath: true
versions: '[7,]'
reviewed: 2026-10-26
related:
 - nservicebus/operations
redirects:
 - nservicebus/azure-storage-queues/performance-tuning
---

## General guidelines

Microsoft's [performance and scalability checklist for Queue Storage](https://learn.microsoft.com/en-us/azure/storage/queues/storage-performance-checklist) has general performance tips and design guidelines.

## Parallel message retrieval

The transport reads messages using several parallel receive operations, each fetching a batch of up to 32 messages. By default, it fetches as many messages as the configured [message processing concurrency](/nservicebus/operations/tuning.md) allows, which means one receive operation for every 32 messages of concurrency. For example, a concurrency of 100 results in four parallel receive operations.

To change this, use the [`DegreeOfReceiveParallelism`](/transports/azure-storage-queues/configuration.md#configuration-parameters-degreeofreceiveparallelism) and [`BatchSize`](/transports/azure-storage-queues/configuration.md#configuration-parameters-batchsize) settings.

> [!NOTE]
> Changing the value of `DegreeOfReceiveParallelism` will influence the total number of storage operations against Azure Storage Services and can result in higher costs.
