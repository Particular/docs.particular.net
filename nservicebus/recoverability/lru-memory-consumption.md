---
title: Avoiding excessive memory consumption
summary: Explains how to avoid excessive memory consumption due to recoverability exception caching
component: Core
versions: '[6, )'
reviewed: 2019-09-09
---

The MSMQ and SQL Server transports cache exceptions in memory to allow transactions to be cleared before executing recoverability policies. Therefore, exceptions with large memory footprints can cause excessive memory consumption. Furthermore, the cache may retain items for longer when the endpoint is scaled out. This page describes how to solve issues caused by excessive memory consumption due to this caching.

## Dispose of exception specific resources

An exception may be caught to explicitly dispose of resources before it is rethrown and cached.

For example, disposing the response body contained in a `WebException` may significantly reduce the amount of memory required to cache the exception:

snippet: dispose-large-exceptions

See [pipeline customization documentation](/nservicebus/pipeline/manipulate-with-behaviors.md) for more details.