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
reviewed: 2026-05-22
---

NServiceBus uses defaults that ensure good performance in common cases. This is usually the preferred mode of operation, but tuning might be required in some situations.

Concurrency tuning might be relevant in these situations:

- Non-thread-safe code that needs to run sequentially
- Databases that might deadlock when handling too many concurrent requests

## Configuring concurrency limit

You can change an endpoint's default concurrency setting in code:

snippet: TuningFromCode

> [!NOTE]
> The default concurrency limit is `max(Number of logical processors, 2)`.

Limit maximum concurrency so that no more than the specified number of messages are processed at the same time. If a maximum concurrency is not specified, the transport chooses an optimal value that balances throughput and effective resource usage. The concurrency limit cannot be changed at runtime. It can only be applied when creating an endpoint instance, and the endpoint instance must be restarted for concurrency changes to take effect.

Set up infrastructure monitoring for the environment that hosts the endpoint instance, as well as remote resources such as databases. Monitor CPU, RAM, network, and storage to validate that concurrency changes are not negatively affecting the overall system.

> [!NOTE]
> The concurrency set in endpoint configuration defines the concurrency of each endpoint instance, not the aggregate concurrency across all endpoint instances. For example, if the endpoint configuration sets concurrency to 4 and the endpoint is scaled out to three instances, the combined concurrency is 12, not 4.

## Parallelism

If you have long-running compute-bound code or synchronous code in your handler and want to achieve higher parallelism, refer to the [calling long-running code in async handlers](/nservicebus/handlers/async-handlers.md#calling-long-running-compute-bound-code) section of the documentation.

## Sequential processing

Set the concurrency limit value to `1` to process messages sequentially. Sequential processing is not a guarantee for ordered processing. For example, processing failures and [recoverability](/nservicebus/recoverability/) will result in out-of-order processing.

> [!NOTE]
> Sequential processing on the endpoint (logical) level is not possible when scaled out.

## Throttling

Throughput throttling options are deprecated. To enable throttling on version 6 and later, use a custom behavior. The [throttling sample](/samples/throttling/) demonstrates how to implement one.

partial: timeoutmanager
