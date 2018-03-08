---
title: Performance Tuning
summary: Tips on how to get the best performance from the Azure Storage Queues transport
component: ASQ
versions: '[7,]'
reviewed: 2018-03-08
tags:
- Azure
- Transport
- Performance
related:
 - nservicebus/operations
redirects:
 - nservicebus/azure-storage-queues/performance-tuning
---

include: azure-storage-performance-tuning


## Parallel message retrieval

Multiple parallel read operations are used to improve message throughput. The amount of parallel read operations is the square root of the configured [message processing concurrency](/nservicebus/operations/tuning.md). This value can be increased or decreased if needed by using the `DegreeOfReceiveParallelism` configuration parameter. See [Azure Storage Queues Transport Configuration](/transports/azure-storage-queues/configuration.md) on how to use this parameter.

Note: Changing the value of `DegreeOfReceiveParallelism` will influence the total number of storage operations against Azure Storage Services and can result in higher costs.
