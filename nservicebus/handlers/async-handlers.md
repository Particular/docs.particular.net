---
title: Asynchronous Handlers
summary: Shows how to deal with synchronous and asynchronous code inside asynchronous handlers
reviewed: 2016-07-05
tags:
 - Async
---


## Introduction

WARN: It is difficult to give generic advice how asynchronous code should be structured. It is important to understand compute-bound vs. IO-bound as summarized above. Avoid copy-pasting snippets without further analysis if they provide benefit for the involved business scenarios. Don't assume. Measure it!

[Handlers](/nservicebus/handlers/) and [Sagas](/nservicebus/sagas/) will be invoked from an IO thread pool thread. Typically message handlers and sagas issue IO bound work like sending or publishing messages, storing information into databases, calling web services and more. In other cases, message handlers are used to schedule compute-bound work. To be able to write efficient message handlers and sagas it is crucial to understand the difference between compute-bound and IO bound work and the thread pools involved.


### Thread Pools

There are two thread pools. The worker thread pool and the IO thread pool.

Further reading: 

 * [Thread Pools](https://msdn.microsoft.com/en-us/library/windows/desktop/ms686760.aspx)
 * [Thread Pooling](https://msdn.microsoft.com/en-us/library/windows/desktop/ms686756.aspx)
 * [IO Completion Ports](https://msdn.microsoft.com/en-us/library/windows/desktop/aa365198.aspx)


#### Worker thread pool

Parallel / Compute-bound blocking work happens on the worker thread pool. Things like [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx), [`Task.Factory.StartNew`](https://msdn.microsoft.com/en-au/library/dd321439.aspx), [`Parallel.For`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.parallel.for.aspx) schedule tasks on the worker thread pool.
On the other hand, if compute bound work is scheduled the worker thread pool will start expanding its worker threads (ramp-up phase). Ramping up more worker threads is expensive. The thread injection rate of the worker thread pool is limited.

**Compute bound recommendations:**

 * Offloading compute-bound work to the worker thread pool is a top-level concern only. Use [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx) or [`Task.Factory.StartNew`](https://msdn.microsoft.com/en-au/library/dd321439.aspx) as high up in the call hierarchy as possible (e.g. in the `Handle` methods of either a [Handler](/nservicebus/handlers/) and [Saga](/nservicebus/sagas/).
 * Avoid those operations deeper down in the call hierarchy.
 * Group compute-bound operations together as much as possible.
 * Make them coarse-grained instead of fine-grained.


#### IO-thread pool

IO-bound work is scheduled on the IO-thread pool. The IO-bound thread pool has a fixed number of worker threads (usually number of cores) which can work concurrently on thousands of IO-bound tasks. IO-bound work under Windows uses so-called [IO completion ports (IOCP)](https://msdn.microsoft.com/en-us/library/windows/desktop/aa365198.aspx) to get notifications when an IO-bound operation is completed. IOCP enable efficient offloading of IO-bound work from the user code to the kernel, driver, and hardware without blocking the user code until the IO work is done. To achieve that the user code registers notifications in the form of a callback. The callback occurs on an IO thread which is a pool thread managed by the IO system that is made available to the user code.

IO-bound work typically takes very long, and compute-bound work is comparatively cheap. The IO system is optimized to keep the thread count low and schedule all callbacks, and therefore the execution of interleaved user code on that one thread. Due to those optimizations, all works get serialized, and there is minimal context switching as the OS scheduler owns the threads. In general asynchronous code can handle bursting traffic much better because of the "always-on" nature of the IOCP.


#### Memory and allocations

Asynchronous code tends to use much less memory because the amount of memory saved by freeing up a thread in the worker thread pool dwarfs the amount of memory used for all the compiler-generated async structures combined.


#### Synchronous vs. Asynchronous

If each request is examined in isolation, an asynchronous code would be slightly slower than the synchronous version. There might be extra kernel transitions, task scheduling, etc. involved. But the scalability more than makes up for it.

From a server perspective if asynchronous is compared to synchronous by just looking at one method or one request at a time, then synchronous might make more sense. But if asynchronous is compared to parallelism - watching the server as a whole - asynchronous wins. Every worker thread that can be freed up on a server is worth freeing up! It reduces the amount of memory needed, frees up the CPU for compute-bound work while saturating the IO system completely.


## Calling short running compute-bound code

Short running compute-bound code (a few milliseconds) that is executed in the handler should be executed directly on the IO-thread that is executing the handler code.

snippet: ShortComputeBoundMessageHandler

Call the code directly and **do not** wrap it with a [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx) or [`Task.Factory.StartNew`](https://msdn.microsoft.com/en-au/library/dd321439.aspx).


## Calling long-running compute-bound code

Long running compute-bound code (more than a hundred milliseconds) that is executed in a handler could be offloaded to the worker thread pool.

snippet: LongComputeBoundMessageHandler

Wrap the compute-bound code in a [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx) or [`Task.Factory.StartNew`](https://msdn.microsoft.com/en-au/library/dd321439.aspx) and `await` the result of the task.


## Return or await


### Awaits the task

For the majority of cases just mark the handler's Handle method with the `async` keyword and `await` all asynchronous calls inside the method.

snippet: HandlerAwaitsTheTask


### Returns the task

For high-throughput scenarios and if there are only one or two asynchronous exit points in the Handle method the `async` keyword can be avoided completely by returning the task instead of awaiting it. This will omit the state machine creation which drives the async code and reduce the number of allocations on the given code path.

snippet: HandlerReturnsATask

snippet: HandlerReturnsTwoTasks


## Usage of ConfigureAwait

By default when a task is awaited a mechanism called context capturing is enabled. The current context is captured and restored for the continuation that is scheduled after the precedent task was completed.

snippet: HandlerConfigureAwaitSpecified

In the snippet above `SomeAsyncMethod` and `AnotherAsyncMethod` are simply awaited. So when entering `SomeAsyncMethod` the context is captured and restored before `AnotherAsyncMethod` is executed. The context capturing mechanism is almost never needed in code that is executed inside handlers or sagas. NServiceBus makes sure the context is not captured in the framework at all. So the following approach is preferred:

snippet: HandlerConfigureAwaitNotSpecified

Specify [`ConfigureAwait(false)`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.configureawait.aspx) on each awaited statement. Apply this principle to all asynchronous code that is called from handlers and sagas.


## Concurrency

Task based APIs enable to better compose asynchronous code and making conscious decisions whether to execute the asynchronous code sequentially or concurrent.


### Small amount of concurrent message operations


#### Batched

By default, all outgoing message operations on the message handler contexts are [batched](/nservicebus/messaging/batched-dispatch.md). Batching means messages are collected in memory and sent out when the handler is completed. So the IO-bound work happens outside the execution scope of a handler (individual transports may apply optimizations). For a few outgoing message operations, it makes sense, to reduce complexity, to sequentially await all the outgoing operations as shown below.

snippet: BatchedDispatchHandler


#### Immediate dispatch

[Immediate dispatch](/nservicebus/messaging/send-a-message.md#immediate-dispatch) means outgoing message operations will be immediately dispatched to the transport of choice. For immediate dispatch operations, it might make sense execute them concurrently like shown below.

snippet: ImmediateDispatchHandler


### Large amount of concurrent message operations

Unbounded concurrency can be problematic. For large numbers of concurrent message operations, it might make sense to package multiple outgoing operations together into batches. Therefore limiting the concurrency to the size of an individual batch (divide & conquer).

snippet: PacketsImmediateDispatchHandler

It is also possible to limit the concurrency by using [`SemaphoreSlim`](https://msdn.microsoft.com/en-us/library/system.threading.semaphoreslim.aspx) like shown below.

snippet: ConcurrencyLimittingImmediateDispatchHandler

In practice packaging operations together has proved to be more effective both in regards to memory allocations and performance. The snippet is shown nonetheless for completeness reasons as well as because [`SemaphoreSlim`](https://msdn.microsoft.com/en-us/library/system.threading.semaphoreslim.aspx) is a useful concurrency primitive for various scenarios.


## Integration with non-tasked based APIs


### Events

Sometimes it is necessary to integrate existing code which fires events into an asynchronous handler. Before async/await was introduced [`ManualResetEvent`](https://msdn.microsoft.com/en-us/library/system.threading.manualresetevent.aspx) or [`AutoResetEvent`](https://msdn.microsoft.com/en-us/library/system.threading.autoresetevent.aspx) were usually used to synchronize runtime code flow. Unfortunately, these synchronization primitives are of blocking nature. For asynchronous one-time event synchronization the [`TaskCompletionSource<TResult>`](https://msdn.microsoft.com/en-us/library/dd449174.aspx) can be used.

snippet: HandlerWhichIntegratesWithEvent

The above snippet shows how a [`TaskCompletionSource<TResult>`](https://msdn.microsoft.com/en-us/library/dd449174.aspx) can be used to asynchronously wait for an event to happen including cancellation support.


### Asynchronous programming model (APM) pattern

For existing code which uses the [Asynchronous Programming Model (APM)](https://msdn.microsoft.com/en-us/library/ms228963.aspx) it is best to use [`Task.Factory.FromAsync`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.taskfactory.fromasync.asp) to wrap the `BeginOperationName` and `EndOperationName` methods with a task object.

snippet: HandlerWhichIntegratesWithAPM


### Asynchronous RPC calls

The APM approach described above can be used to integration with remote procedure calls as shown in this snippet:

snippet: HandlerWhichIntegratesWithRemotingWithAPM

or use [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx) directly in message handler:

snippet: HandlerWhichIntegratesWithRemotingWithTask

NOTE: [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx) can have significantly less overhead than using a delegate with `BeginInvoke`/`EndInvoke`. By default, both APIs will use the worker thread pool as the underlying scheduling engine. Analyze and measure for the business scenarios involved.