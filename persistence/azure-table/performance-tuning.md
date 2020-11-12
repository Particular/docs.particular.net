---
title: Performance Tuning
summary: Tips on how to get the best performance from the Azure Storage Queues persistence
reviewed: 2020-03-25
component: ASP
related:
 - nservicebus/operations
redirects:
 - nservicebus/azure-storage-persistence/performance-tuning
 - persistence/azure-storage/performance-tuning
---

## General guidelines

Microsoft's [Azure Storage Performance Checklist](https://docs.microsoft.com/en-us/azure/storage/storage-performance-checklist) should be considered for performance tips and design guidelines.

## Disabled secondary index scanning when creating new sagas

A secondary index record was not created by the persister contained in the `NServiceBus.Azure` package. To provide backward compatibility, the `NServiceBus.Persistence.AzureStorage` package performs a full table scan across all partitions for secondary index records before creating a new saga. For systems that have only used the `NServiceBus.Persistence.AzureStorage` library, or have verified that all saga instances have a secondary index record, full table scans can be safely disabled by using the [AssumeSecondaryIndicesExist](/persistence/azure-storage/configuration.md#configuration-properties-saga-configuration) setting.