---
title: Asynchronous Handlers
summary: Shows how to deal with synchronous and asynchronous code inside asynchronous handlers
tags:
 - Async
redirects:
---

### Introduction

Message handlers and Sagas will be invoked from an IO thread pool thread. Typically message handlers and sagas issue IO bound work like sending or publishing messages, storing information into databases, callling webservices and more. In other cases message handlers are used to schedule compute-bound work. To be able to write efficient message handlers and sagas it is crucial to understand the difference between compute-bound and IO bound work and the thread pools involved.

#### Threadpools

There are basically two thread pools. The worker thread pool and the IO thread pool.

##### Worker thread pool
Parallel / Compute-bound blocking work happens on the worker thread pool. Things like `Task.Run`, `Task.Factory.StartNew`, `Parallel.For` schedule tasks on the worker thread pool.
On the other hand if we schedule Compute bound work the worker thread pool will start expanding its worker threads. Let's call this phase ramp up phase. Ramping up more worker threads is expensive. The thread injection rate of the worker thread pool is limited.

Offloading compute-bound work to the worker thread pool is a top-level concern only. Use `Task.Run` or `Task.Factory.StartNew` is high up in the hierarchy as possible (i.ex. in the message handler). Avoid those operations deeper down in the call stack. Group compute-bound operations together as much as possible. Make them coarse-grained instead of fine-grained.

##### IO-thread pool
IO-bound work is scheduled on the IO-thread pool. The IO-bound thread pool has a fixed number of worker threads (usually number of cores) which can work concurrently on thousands of IO-bound tasks. IO-bound work under Windows uses so called IO completion ports (IOCP) to get notifications when an IO-bound operation is completed. IOCP enable efficient offloading of IO-bound work from the user code to the kernel, driver and hardware without blocking the user code until the IO work is done. To achieve that the user code registeres notifications in form of a callback. The callback occors on an IO thread which is a pool thread managed by the IO system that is made available to the user code.

IO-bound work typically takes very long and compute-bound work is comparatively cheap. The IO system is optimized to keep the thread count low and schedule all callbacks, and therefore the execution of interleaved user code on that one thread. Due to those optimizations all works get serialized and there is minimal context switching as the OS scheduler owns the threads. In general asynchronous code can handle bursting traffic much better because of the "always-on" nature of the IOCP.

##### Memory and allocations

Asynchronous code tends to use much less memory because the amount of memory saved by freeing up a thread in the worker thread pool dwarfs the amount of memory used for all the compiler-generated async structures combined.

##### Sync vs. async
If we'd examine each request in isolation, asynchronous code would actually be sligthly slower than the synchronous version. There might be extra kernel transitions, task scheduling etc. involved. But the scalability more than makes up for it.

From a server perspective if we compare asynchrony to synchrony by just looking at one method or one request at a time then synchrony might make more sense. But if asynchrony is compared to parallelism - looking at the server as a whole - asynchrony generally wins. Every worker thread that can be freed up on a server is worth freeing up! It reduces the amount of memory needed, frees up the CPU for compute-bound work while saturating the IO system completely.

WARN: It is generally hard to give generic advice how asynchronous code should be structured. It is important to understand compute-bound vs. IO-bound like summarized above. Avoid to copy paste the snippets blindly without further analysis if they actually provide benefit for the involved business scenarios. Don't assume. Measure it!

### Calling short running compute-bound code

Short running compute-bound code (a few milliseconds) that is executed in the handler should be executed directly on the IO-thread that is executing the handler code.

snippet: ShortComputeBoundMessageHandler

Call the code directly and do not wrap it with a `Task.Run` or `Task.Factory.StartNew`.

### Calling long running compute-bound code

Long running compute-bound code (more than hundred milliseconds) that is executed in a handler could be offloaded to the worker thread pool.

snippet: LongComputeBoundMessageHandler

Wrap the compute-bound code in a `Task.Run` or `Task.Factory.StartNew` and `await` the result of the task.

### Return or await

#### Awaits the task

For the majority of cases just mark the handler's Handle method with the `async` keyword and `await` all asynchronous calls inside the method.

snippet: HandlerAwaitsTheTask

#### Returns the task

For high-troughput scenarios and if you have only one or two asynchronous exit points in the Handle method you can avoid the `async` keyword by returning the task instead of awaiting it. This will omit the state machine creation which drives the async code and reduce the number of allocations on the given code path.

snippet: HandlerReturnsATask

snippet: HandlerReturnsTwoTasks

### Usage of `ConfigureAwait`

By default when a task is awaited a mechanism called context capturing is enabled. The current context is captured and restored for the continuation that is scheduled after the precedent task was completed.

snippet: HandlerConfigureAwaitSpecified

In the snippet above `SomeAsyncMethod` and `AnotherAsyncMethod` are simply awaited. So when entering `SomeAsyncMethod` the context is captured and restored before `AnotherAsyncMethod` is executed. The context capturing mechanism is almost never needed in code that is executed inside handlers or sagas. NServiceBus makes sure the context is not captured in the framework at all. So the following approach is preferred:

snippet: HandlerConfigureAwaitNotSpecified

Specify `ConfigureAwait(false)` on each awaited statement. Apply this principle to all asynchronous code that is called from handlers and sagas.

### Concurrency

Task based APIs enable to better compose asynchronous code and making conscious decisions whether to execute the asynchronous code sequentially or concurrent.

#### Small amount of concurrent message operations

By default all outgoing message operations on the message handler contexts are batched (READ DOCO HERE). In summary batched means they are collected in memory and sent out when the handler is completed. So the IO-bound work happens outside the execution scope of a handler (individual transports may apply optimizations). For a small amount of outgoing message operations it makes sense, in order to reduce complexity, to sequentially await all the outgoing operations like shown below.  

snippet: BatchedDispatchHandler

Immediate dispatch (READ DOCO HERE) means outgoing message operations will be immediately dispatched to the transport of choice. For immediate dispatch operations it might make sense execute them concurrently like shown below.

snippet: ImmediateDispatchHandler

#### Large amount of concurrent message operations

Unbounded concurrency can be problematic. For large numbers of concurrent message operations it might make sense to package multiple outgoing operations together into batches. Therefore limiting the concurrency to the size of an individual batch (divide & conquer).

snippet: PacketsImmediateDispatchHandler

It is also possible to limit the concurrency by using `SemaphoreSlim` like shown below.

snippet: ConcurrencyLimittingImmediateDispatchHandler

In practice packaging operations together has shown to be more effective both in regards to memory allocations and performance. The snippet is shown nonetheless for completness reasons as well as because `SemaphoreSlim` is a useful concurrency primitive for various scenarios.

### Integration with non-tasked based APIs

#### Events

snippet: HandlerWhichIntegratesWithEvent

#### Asynchronous programming model pattern

snippet: HandlerWhichIntegratesWithAPM

#### Asynchronous RPC calls

snippet: HandlerWhichIntegratesWithRemoting
