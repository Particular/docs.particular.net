---
title: Cooperative cancellation
summary: To participate in graceful shutdown initiated by the host
reviewed: 2021-05-19
component: core
---

As of Version 8 NServiceBus supports [cooperative cancellation](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation). This enables NServiceBus to participate in graceful shutdown of the host by allowing the use of [cancellation tokens](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) to abort potentially long-running operations.

## Inside message handlers

Since NServiceBus are in control of the invocation of message handlers it's the stopping of the endpoint instance that signals the start of the shutdown sequence. `Endpoint.Stop` will by default allow all in-fligt message handlers to complete but the user can now abort handler invocations via an optional cancallation token.

This token is exposed to the message handlers via `IMessageHandlerContext.CancellationToken` and must be observed by all potential long running calls made by the message handler as show below:

TBD: add snippet

NServiceBus ships with an analyzer that will make users aware when the token isn't passed to methods that does accept a cancellation token.

TBD: flesh out

## Outside message handlers

Performing messaging operations [outside of message handlers](/nservicebus/messaging/send-a-message.md#outside-a-message-handler) via the `IMessageSession` now all accepts an optional cancellation token.

Examples here could be a Asp.net web request handler sending a message.
