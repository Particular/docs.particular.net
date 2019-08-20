---
title: Avoiding excessive memory consumption
summary: Explains how to avoid excessive memory consumption due to recoverability exception caching
component: Core
tags:
 - Exceptions
 - Error Handling
 - Retry
 - Recoverability
reviewed: 2019-08-16
---

MSMQ and SQL Server transport needs to cache exceptions in memory to allow transactions to be cleared before executing recoverability policies. Therefore, exceptions with a large memory footprint can cause high memory usage of the NServiceBus process. Further, the recoverability cache might hold on to items longer when endpoints are scaled out. This page describes how to solve issues caused by excessive memory consumption due to this caching.

## Dispose exception specific resources

Large exceptions which cause high memory consumption can be handled manually to explicitly dispose resources before they will be cached. This will reduce the overall memory stored in the cache.

The following sample shows how to dispose large response bodies contained in `WebException`s to eliminate unnecessary resource usage:

snippet: dispose-large-exceptions

See [pipeline customization documentation](/nservicebus/pipeline/manipulate-with-behaviors.md) for more details.
