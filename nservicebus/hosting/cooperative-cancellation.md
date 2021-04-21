---
title: Cooperative cancellation
summary: To participate in graceful shutdown initiated by the host
reviewed: 2021-05-19
component: core
---

As of Version 8 NServiceBus supports [cooperative cancellation](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation). This enables NServiceBus to participate in graceful shutdown of its host by exposing a [cancellation token](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) to abort potentially long-running operations both inside and outside of message handlers.

### Non-message-handling contexts

Methods used outside the message processing pipeline include an optional `CancellationToken` parameter, including methods for starting and stopping endpoints, and methods to send or publish messages from outside a message processing pipeline, such as from within a web application.

It is recommended to forward a `CancellationToken` to any method that accepts one. For example, within a web application controller:

snippet: cancellation-token-in-asp-controller

[Enabling .NET source code analysis](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview) (enabled by default in projects targeting .NET 5 or above) is also recommended so that [CA2016: Forward the CancellationToken parameter to methods that take one](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2016) can identify locations where the `CancellationToken` should be forwarded. This rule is presented as an informational message only, but the [analyzer severity](https://docs.microsoft.com/en-us/visualstudio/code-quality/use-roslyn-analyzers#configure-severity-levels) can be upgraded to a warning using an [.editorconfig file](https://editorconfig.org/):

```ini
[*.cs]
dotnet_diagnostic.CA2016.severity = warning
```

### Message handlers and pipeline

Inside a message handler, a cancellation token from the incoming message processing pipeline is available on the `IMessageHandlerContext` as the property `context.CancellationToken`. The cancellation token was added to the context parameter to avoid making a breaking change to `IHandleMessages` affecting all message handlers and sagas.

Similarly, [pipeline behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md) also contain a `CancellationToken` property on their respective `context` parameters.

Methods on `IMessageHandlerContext` such as `Send()` and `Publish()` do not accept a cancellation token, as the token from the incoming message pipeline will be routed to the outgoing operations transparently.

It is recommended to forward the `context.CancellationToken` to any other method that accepts one. A new analyzer identifies locations where the token should be forwarded with a build warning, for example:

snippet: cancellation-token-in-message-handler

The analyzer also offers a code fix that will update the code to forward the token using the "light bulb" menu. ( <kbd>Ctrl</kbd> + <kbd>.</kbd> )

If cancellation is not a major concern, the [analyzer severity](https://docs.microsoft.com/en-us/visualstudio/code-quality/use-roslyn-analyzers#configure-severity-levels) can be downgraded using an [.editorconfig file](https://editorconfig.org/):

```ini
[*.cs]
dotnet_diagnostic.NSB0002.severity = suggestion
```
