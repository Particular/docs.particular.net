---
title: Performance Tuning
summary: Tips on how to get the best performance from the Azure Storage Queues transport
component: ASQ
isLearningPath: true
versions: '[7,]'
reviewed: 2020-03-25
related:
 - nservicebus/operations
redirects:
 - nservicebus/azure-storage-queues/performance-tuning
---

## General guidelines

Microsoft's [Azure Storage Performance Checklist](https://docs.microsoft.com/en-us/azure/storage/storage-performance-checklist) should be considered for performance tips and design guidelines.

## Parallel message retrieval

Multiple parallel read operations are used to improve message throughput. The amount of parallel read operations is the square root of the configured [message processing concurrency](/nservicebus/operations/tuning.md). This value can be increased or decreased as needed by using the `DegreeOfReceiveParallelism` configuration parameter. See [Azure Storage Queues Transport Configuration](/transports/azure-storage-queues/configuration.md) on how to use this parameter.

Note: Changing the value of `DegreeOfReceiveParallelism` will influence the total number of storage operations against Azure Storage Services and can result in higher costs.