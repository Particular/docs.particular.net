---
title: Asynchronous Handlers
summary: How to deal with synchronous and asynchronous code inside asynchronous handlers
reviewed: 2018-03-13
component: Core
versions: '[6.0,)'
---


WARNING: It is difficult to give generic advice on how asynchronous code should be structured. It is important to understand compute-bound vs. I/O-bound operations and avoid copying and pasting snippets without further analysis if they provide benefit for the involved business scenarios. Don't assume; measure it.

[Handlers](/nservicebus/handlers/) and [sagas](/nservicebus/sagas/) will be invoked from a thread in the thread pool. Depending on the transport implementation a worker thread pool thread or an I/O thread pool thread might be used. Typically message handlers and sagas issue I/O-bound work, such as sending or publishing messages, storing information into databases, and calling web services. In other cases, message handlers are used to schedule compute-bound work. To be able to write efficient message handlers and sagas, it is crucial to understand the difference between compute-bound and I/O-bound work.


### Thread pool

A thread pool is associated with a process and manages the execution of asynchronous callbacks on behalf of the application. Its primary purpose is to reduce the number of application threads and provide efficient management of threads. The thread pool distinguishes asynchronous callbacks into two categories: I/O-bound and compute-bound. It manages a pool of threads which are associated with one of these categories.

Further reading:

 * [Overview of the Threadpool](https://msdn.microsoft.com/en-us/magazine/ff960958.aspx)
 * [CLR 4.0 Threadpool improvements](https://blogs.msdn.microsoft.com/ericeil/2009/04/23/clr-4-0-threadpool-improvements-part-1/)
 * [Thread Pools](https://msdn.microsoft.com/en-us/library/windows/desktop/ms686760.aspx)
 * [Thread Pooling](https://msdn.microsoft.com/en-us/library/windows/desktop/ms686756.aspx)
 * [I/O Completion Ports](https://msdn.microsoft.com/en-us/library/windows/desktop/aa365198.aspx)


#### Worker thread pool

Parallel / Compute-bound blocking work happens on the worker thread pool. Things like [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx), [`Task.Factory.StartNew`](https://msdn.microsoft.com/en-au/library/dd321439.aspx), [`Parallel.For`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.parallel.for.aspx) schedule tasks on the worker thread pool.

Alternatively, if compute-bound work is scheduled, the worker thread pool will start expanding its worker threads (ramp-up phase). Ramping up more worker threads is expensive. The thread injection rate of the worker thread pool is limited.

**Compute-bound recommendations:**

 * Offloading compute-bound work to the worker thread pool is a top-level concern only. Use [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx) or [`Task.Factory.StartNew`](https://msdn.microsoft.com/en-au/library/dd321439.aspx) as high up in the call hierarchy as possible (e.g. in the `Handle` methods of either a [handler](/nservicebus/handlers/) or [saga](/nservicebus/sagas/).
 * Avoid those operations deeper in the call hierarchy.
 * Group compute-bound operations together as much as possible.
 * Make compute-bound operations coarse-grained instead of fine-grained.


#### I/O-thread pool

I/O-bound work is scheduled on the I/O-thread pool. The I/O-bound thread pool has a fixed number of worker threads (usually equal to the number of cores) which can work concurrently on thousands of I/O-bound tasks. I/O-bound work under Windows uses so-called [I/O completion ports (IOCP)](https://msdn.microsoft.com/en-us/library/windows/desktop/aa365198.aspx) to get notifications when an I/O-bound operation is completed. IOCP enables efficient offloading of I/O-bound work from the user code to the kernel, driver, and hardware without blocking the user code until the I/O work is done. To achieve that, the user code registers notifications in the form of a callback. The callback occurs on an I/O thread which is a pool thread managed by the I/O system that is made available to the user code.

I/O-bound work typically takes very long, and compute-bound work is comparatively cheap. The I/O system is optimized to keep the thread count low and schedule all callbacks, and therefore the execution of interleaved user code on that one thread. Due to those optimizations, all work gets serialized, and there is minimal context switching as the OS scheduler owns the threads. In general, asynchronous code can handle bursting traffic much better because of the "always-on" nature of the IOCP.


#### Memory and allocations

Asynchronous code tends to use much less memory because the amount of memory saved by freeing up a thread in the worker thread pool dwarfs the amount of memory used for all the compiler-generated async structures combined.


#### Synchronous vs. asynchronous

If each request is examined in isolation, asynchronous code would be slightly slower than the corresponding synchronous version. There might be extra kernel transitions, task scheduling, etc. involved but the scalability more than makes up for it.

From a server perspective, if asynchronous code is compared to synchronous code by looking at one method or one request at a time, then synchronous might make more sense. But if asynchronous code is compared to parallelism - watching the server as a whole - asynchronous wins. Every worker thread that can be freed up on a server is worth freeing up. It reduces the amount of memory needed and frees up the CPU for compute-bound work while saturating the I/O system completely.

## Calling short-running, compute-bound code

Short-running, compute-bound code that is executed in the handler should be executed directly on the I/O-thread that is executing the handler code.

snippet: ShortComputeBoundMessageHandler

Call the code directly and **do not** wrap it with a [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx) or [`Task.Factory.StartNew`](https://msdn.microsoft.com/en-au/library/dd321439.aspx).

For the majority of business scenarios, this approach is acceptable since many of the asynchronous base class library methods in the .NET Framework will schedule continuations on the worker thread pool; the likelihood that no I/O-thread is blocked is high.


## Calling long-running, compute-bound code

WARNING: This approach should be used only after a thorough analysis of the runtime behavior and the code involved in the call hierarchy of a handler. Wrapping code inside the handler with `Task.Run` or `Task.Factory.StartNew` can seriously harm the throughput if applied incorrectly. It should be used when multiple long-running compute-bound tasks need to be executed in parallel.

Long-running compute-bound code that is executed in a handler could be offloaded to the worker thread pool.

snippet: LongComputeBoundMessageHandler

Wrap the compute-bound code in a [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx) or [`Task.Factory.StartNew`](https://msdn.microsoft.com/en-au/library/dd321439.aspx) and `await` the result of the task.


## Return or await

### Await the task

For the majority of cases, it is sufficient to mark the handler's `Handle` method with the `async` keyword and `await` all asynchronous calls inside the method.

snippet: HandlerAwaitsTheTask


### Return the task

For high-throughput scenarios and if there are only one or two asynchronous exit points in the Handle method, the `async` keyword can be avoided completely by returning the task instead of awaiting it. This will omit the state machine creation which drives the async code and reduce the number of allocations on the given code path.

snippet: HandlerReturnsATask

snippet: HandlerReturnsTwoTasks


## Usage of ConfigureAwait

By default, when a task is awaited, a mechanism called context-capturing is enabled. The current context is captured and restored for the continuation that is scheduled after the precedent task was completed.

snippet: HandlerConfigureAwaitNotSpecified

In the snippet above, `SomeAsyncMethod` and `AnotherAsyncMethod` are awaited. When entering `SomeAsyncMethod`, the context is captured and restored before `AnotherAsyncMethod` is executed. The context-capturing mechanism is almost never needed in code that is executed inside handlers or sagas. NServiceBus makes sure the context is not captured in the framework at all. So the following approach is preferred:

snippet: HandlerConfigureAwaitSpecified

Specify [`ConfigureAwait(false)`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.configureawait.aspx) on each awaited statement. Apply this principle to all asynchronous code that is called from handlers and sagas.


## Concurrency

Task-based APIs enable better composition of asynchronous code and allow conscious decisions on whether to execute the asynchronous code sequentially or concurrently.


### Small amount of concurrent message operations


#### Batched

By default, all outgoing message operations on the message handler contexts are [batched](/nservicebus/messaging/batched-dispatch.md). Batching means messages are kept in memory and sent out when the handler is completed. So the I/O-bound work happens outside the execution scope of a handler (individual transports may apply optimizations). For a few outgoing message operations it makes sense, to reduce complexity, to sequentially await all the outgoing operations as shown below.

snippet: BatchedDispatchHandler


#### Immediate dispatch

[Immediate dispatch](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately) means outgoing message operations will be immediately dispatched to the underlying transport. For immediate dispatch operations, it might make sense to execute them concurrently as shown below.

snippet: ImmediateDispatchHandler


### Large amount of concurrent message operations

Unbounded concurrency can be problematic. For large numbers of concurrent message operations, it might be preferable to package multiple outgoing operations together into batches, limiting the concurrency to the size of an individual batch (divide & conquer).

snippet: PacketsImmediateDispatchHandler

It is also possible to limit the concurrency by using [`SemaphoreSlim`](https://msdn.microsoft.com/en-us/library/system.threading.semaphoreslim.aspx) as shown below.

snippet: ConcurrencyLimittingImmediateDispatchHandler

In practice, packaging operations together has proven to be more effective both in regards to memory allocations and performance. The snippet is shown nonetheless for completeness reasons as well as because [`SemaphoreSlim`](https://msdn.microsoft.com/en-us/library/system.threading.semaphoreslim.aspx) is a useful concurrency primitive for various scenarios.


## Integration with non-tasked based APIs


### Events

Sometimes it is necessary to call APIs from an asynchronous handler that uses events as the trigger for completion. Before async/await was introduced, [`ManualResetEvent`](https://msdn.microsoft.com/en-us/library/system.threading.manualresetevent.aspx) or [`AutoResetEvent`](https://msdn.microsoft.com/en-us/library/system.threading.autoresetevent.aspx) were usually used to synchronize runtime code flow. Unfortunately, these synchronization primitives are of a blocking nature. For asynchronous one-time event synchronization, the [`TaskCompletionSource<TResult>`](https://msdn.microsoft.com/en-us/library/dd449174.aspx) can be used.

snippet: HandlerWhichIntegratesWithEvent

The above snippet shows how a [`TaskCompletionSource<TResult>`](https://msdn.microsoft.com/en-us/library/dd449174.aspx) can be used to asynchronously wait for an event to happen and optionally cancel it.


### Asynchronous programming model (APM) pattern

For existing code which uses the [Asynchronous Programming Model (APM)](https://msdn.microsoft.com/en-us/library/ms228963.aspx), it is best to use [`Task.Factory.FromAsync`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.taskfactory.fromasync.aspx) to wrap the `BeginOperationName` and `EndOperationName` methods with a task object.

snippet: HandlerWhichIntegratesWithAPM


### Asynchronous RPC calls

The APM approach described above can be used to integrate with remote procedure calls as shown in this snippet:

snippet: HandlerWhichIntegratesWithRemotingWithAPM

or use [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx) directly in a message handler:

snippet: HandlerWhichIntegratesWithRemotingWithTask

NOTE: [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx) can have significantly less overhead than using a delegate with `BeginInvoke`/`EndInvoke`. By default, both APIs will use the worker thread pool as the underlying scheduling engine. Analyze and measure for the business scenarios involved.
