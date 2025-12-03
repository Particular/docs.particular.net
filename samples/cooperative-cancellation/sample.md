---
title: Cooperative cancellation
summary: Stopping both the endpoint and a long running handler with a cancellation token
reviewed: 2025-10-14
component: Core
---

This sample demonstrates graceful shutdown of its host via [cooperative cancellation](/nservicebus/hosting/cooperative-cancellation.md) by signaling a [cancellation token](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) to abort a simulated long-running async operation inside a handler.

To get started run the solution. A single console application starts up: `Server`.

## Code walk-through

When the endpoint is started, a message is sent to the endpoint, which triggers a long-running message handler.  The handler enters an infinite loop, logging a message every two seconds, by calling the async `Task.Delay()` operation.  The `CancellationToken` provided by the message handling context object is passed to `Task.Delay()` to cancel the delay operation if the `context.CancellationToken.IsCancellationRequested` property is set to `true`.

> [!NOTE]
> The NServiceBus pipeline filters out the `OperationCanceledException` specifically when the `CancellationToken.IsCancellationRequested` property is set to `true`. Further details on this behavior can be found in the [cancellation and catching exceptions](/nservicebus/cancellation-and-catching-exceptions.md) documentation.

snippet: LongRunningMessageHandler

Once a key is pressed, a `CancellationTokenSource` is created, scheduling a cancel operation after one second.  This cancellation token is then passed to the endpoint's stop command for cooperative cancellation. (See the blog post [Cancellation in NServiceBus 8](https://particular.net/blog/cancellation-in-nservicebus-8).)

snippet: StoppingEndpointWithCancellationToken

After one second, a signal is sent to the cancellation token, terminating the long running handler and forcibly shutting down the endpoint. If the handler were to complete its operation before the cancel signal is sent to the cancellation token, then the endpoint would gracefully shutdown.
