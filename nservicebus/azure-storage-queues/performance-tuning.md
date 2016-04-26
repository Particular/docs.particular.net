---
title: Azure Storage Queues Transport Performance Tuning
summary: Tuning Azure Storage Queues Transport performance
reviewed: 2016-04-21
tags:
- Azure
- Cloud
- Transport
- Performance
---

include: azure-storage-performance-tuning


## Parallel Message Retrieval

In NServiceBus Azure Storage Queue Transport Version 7 and above, multiple parallel read operations are used to improve message throughput. The amount of parallel read operations is the square root of the configured [message processing concurrency](/nservicebus/operations/tuning.md). This value can be increased or decreased if needed by using the `DegreeOfReceiveParallelism` configuration parameter. See [Azure Storage Queues Transport Configuration](/nservicebus/azure-storage-queues/configuration.md) on how to use this parameter.

Note: Changing the value of `DegreeOfReceiveParallelism` will influence the total number of storage operations against Azure Storage Services and can result in higher costs.
