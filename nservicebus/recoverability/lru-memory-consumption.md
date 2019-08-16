---
title: Avoiding excessive memory consumption
summary: Explains how avoid excessive memory consumption due to recoverability exception caching
component: Core
tags:
 - Exceptions
 - Error Handling
 - Retry
 - Recoverability
reviewed: 2019-08-16
---

MSMQ and SQL Server transport need to cache exceptions in memory for immediate retries. Therefore, exceptions with a large memory footprint can cause high memory usage of the NServiceBus process. The recoverability cache might require more resources when scaling out endpoints. This page describes how to solve issues caused by excessive memory consumption due to recoverability's caching mechanism.

## Dispose exception specific resources

## Disable immediate retries

## Lower cache size (not available)