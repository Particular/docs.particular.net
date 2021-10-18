---
title: Tuning endpoint message processing
summary: Optimize message processing by configuring concurrency
component: Core
related:
 - nservicebus/operations
redirects:
 - nservicebus/how-to-reduce-throughput-of-an-endpoint
 - nservicebus/operations/reducing-throughput
 - nservicebus/operations/throughput
reviewed: 2020-06-23
---

NServiceBus uses defaults that ensure good performance in common cases. While this is usually the preferred mode of operation there are situations where tuning might be desired.

## Tuning concurrency

partial: defaults

Limit maximum concurrency so that no more messages than the specified value are ever processed at the same time. If a maximum concurrency is not specified, the transport will choose an optimal value that is a balance between throughput and effective resource usage.

NOTE: Concurrency is configured per endpoint instance. If the concurrency is set to 4 on each instance and the endpoint is scaled-out to 3 instances the combined concurrency is 12. It is not possible to have sequential processing when scaled-out.

Sequential processing:

Set the concurrently limit value to `1` to process messages sequentially.

NOTE: Sequential processing is not a guarantee for ordered processing. For example, processing failures and [/nservicebus/recoverability] can result in out-of-order processing.

NOTE: Sequential processing on the endpoint (logical) level is not possible when scaled-out.

Examples where concurrency tuning might be relevant are:

 * Non thread safe code that needs to run sequentially
 * Databases that might deadlock when getting too many concurrent requests
 
NOTE: The endpoint needs to be restarted for concurrency changes to take effect.

## Throttling

partial: throttling

## Configuration

partial: configuration

partial: runtime
