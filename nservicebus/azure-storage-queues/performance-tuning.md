---
title: Azure Storage Transport Performance Tuning
summary: Performance Tuning Azure Storage as transport
reviewed: 2016-04-21
tags:
- Azure
- Cloud
- Persistence
- Transport
---

include: azure-storage-performance-tuning


## Parallel Message Retrieval

To improve message throughput, Azure Storage Queue Transport uses multiple parallel read operations. The amount of parallel read operations is the square root of the configured [message processing concurrency](/nservicebus/operations/tuning.md). You can increase or decrease this value if needed by using the `DegreeOfReceiveParallelism` configuration parameter. See [Azure Storage Queues Transport Configuration](/nservicebus/azure-storage-queues/configuration.md) on how to use this parameter.

Note: Changing the value of `DegreeOfReceiveParallelism` will influence the total number of storage operations against Azure Storage Services and can result in higher costs.
