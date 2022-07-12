---
title: Tuning message throughput performance and concurrency
summary: Optimize message throughput performance by configuring concurrency
component: Core
related:
 - nservicebus/operations
redirects:
 - nservicebus/how-to-reduce-throughput-of-an-endpoint
 - nservicebus/operations/reducing-throughput
 - nservicebus/operations/throughput
reviewed: 2021-11-01
---

NServiceBus uses defaults that ensure good performance in common cases. While this is usually the preferred mode of operation there are situations where tuning might be desired.

## Tuning concurrency

Examples where concurrency tuning might be relevant are:

* Non-thread-safe code that needs to run sequentially
* Databases that might deadlock when getting too many concurrent requests

## Configuring concurrency limit

partial: defaults

Limit maximum concurrency so that no more messages than the specified value are ever processed at the same time. If a maximum concurrency is not specified, the transport will choose an optimal value that is a balance between throughput and effective resource usage. The concurrency limit cannot be changed at run-time and can only be applied at endpoint instance creation and requires the instance to be restarted for concurrency changes to take effect.

Infrastructure monitoring should be set up for the environment that hosts the endpoint instance (as well as any remote resources, such as databases) to monitor CPU, RAM, network, and storage to validate if a change made to the concurrency is not negatively affecting the overall system.

NOTE: The concurrency set in the endpoint configuration defines the concurrency of each endpoint instance, and not the aggregate concurrency across all endpoint instances. For example, if the endpoint configuration sets the concurrency to 4 and the endpoint is scaled-out to 3 instances, the combined concurrency will be 12 and not 4.

## Sequential processing

Set the concurrency limit value to `1` to process messages sequentially. Sequential processing is not a guarantee for ordered processing. For example, processing failures and [recoverability](/nservicebus/recoverability/) will result in out-of-order processing.

NOTE: Sequential processing on the endpoint (logical) level is not possible when scaled-out.

## Throttling

partial: throttling

## Configuration

partial: configuration

partial: runtime
