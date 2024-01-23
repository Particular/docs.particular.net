---
title: Cooperative cancellation
summary: Stopping both the endpoint and a long running handler with a cancellation token
reviewed: 2024-01-20
component: Core
---

Run the solution. A single console application starts up: `Server`.

## Code walk-through

When the endpoint is started, a message is sent to the endpoint, which triggers a long-running message handler that enters a loop simulating a task that takes 3 seconds to execute, like writing a bunch of records to a database, by calling the async `Task.Delay()` operation.

The `try-catch` block is only being used to demonstrate how the async `Task.Delay()` operation throws an `OperationCancelException` when `context.CancellationToken.IsCancellationRequested` is set to `true`.  In most cases, [catching exceptions within a message handler](/nservicebus/cancellation-and-catching-exceptions.md#inside-the-message-processing-pipeline) is discouraged.

Note: The NServiceBus pipeline filters out the `OperationCanceledException`.

snippet: LongRunningMessageHandler

Once a key is pressed, a `CancellationTokenSource` is created, scheduling a cancel operation after one second.  This cancellation token is then passed to the endpoint's stop command for cooperative cancellation. (See the blog post [Cancellation in NServiceBus 8](https://particular.net/blog/cancellation-in-nservicebus-8).)

snippet: StoppingEndpointWithCancellationToken

After one second, a signal is sent to the cancellation token, terminating the long running handler and forcibly shutting down the endpoint. If the handler were to complete its operation before the cancel signal is sent to the cancellation token, then the endpoint would gracefully shutdown.
