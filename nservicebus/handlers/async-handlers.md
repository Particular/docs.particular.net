---
title: Asynchronous Handlers
summary: Shows how to deal with synchronous and asynchronous code inside asynchronous handlers
tags:
 - Async
redirects:
---

### Calling short running compute-bound code

snippet: ShortComputeBoundMessageHandler

### Calling long running compute-bound code

snippet: LongComputeBoundHandler

### Return or await

#### Returns the task

snippet: HandlerReturnsATask

#### Awaits the task

snippet: HandlerAwaitsTheTask

### Usage of `ConfigureAwait`

snippet: HandlerConfigureAwaitSpecified

snippet: HandlerConfigureAwaitNotSpecified

### Concurrency

#### Small amount of concurrent message operations

Brain dump:
By default batched. Concurreny only makes sense if the outgoing pipeline is customized by user or third party and executes actual IO-bound operations inside the batched part of the outgoing pipeline. So for batched dispatch, do not use concurrent execution (the overhead would outweight the benefit):

snippet: BatchedDispatchHandler

For immediate dispatch it might make sense

snippet: ImmediateDispatchHandler

#### Large amount of concurrent message operations

Brain dump: You can schedule packets of immediate dispatches, usually most efficient one

snippet: PacketsImmediateDispatchHandler

Brain dump: You can limit concurrency

snippet: ConcurrencyLimittingImmediateDispatchHandler
