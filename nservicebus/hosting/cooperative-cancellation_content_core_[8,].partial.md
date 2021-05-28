## Outside the message processing pipeline

Methods used outside the message processing pipeline have an optional `CancellationToken` parameter. This includes methods for starting and stopping endpoints, and methods used to send or publish messages from outside a message processing pipeline, such as a web application.

`CancellationToken` parameters should be forwarded to any method that accepts one. For example, within a web application controller:

snippet: cancellation-token-in-asp-controller

Enabling [.NET source code analysis](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview) (enabled by default in projects targeting .NET 5 or later) is also recommended so that [CA2016: Forward the CancellationToken parameter to methods that take one](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2016) can identify locations where a `CancellationToken` should be forwarded, and offers a code fix which updates the analyzed code to forward the token.

By default, violations of this rule are shown only as informational messages, but the [analyzer severity](https://docs.microsoft.com/en-us/visualstudio/code-quality/use-roslyn-analyzers#configure-severity-levels) can be upgraded to a warning using an [`.editorconfig` file](https://editorconfig.org/):

```ini
[*.cs]
dotnet_diagnostic.CA2016.severity = warning
```

## Inside the message processing pipeline

Inside a message handler or saga, a cancellation token from the incoming message processing pipeline is available on the `IMessageHandlerContext` as the `context.CancellationToken` property. The cancellation token was added to the context parameter to avoid making a breaking change to `IHandleMessages`, which would affect all message handlers and sagas.

Similarly, [pipeline behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md) also contain a `CancellationToken` property on their respective `context` parameters.

Methods on `IMessageHandlerContext` such as `Send()` and `Publish()` do not accept a cancellation token. The token from the incoming message pipeline is implicitly routed to outgoing operations.

The `context.CancellationToken` should be forwarded to any other method that will accept it. An analyzer included with NServiceBus identifies locations where the token should be forwarded with a build warning. For example:

snippet: cancellation-token-in-message-handler

The analyzer also offers a code fix that will update the code to forward the token using the "light bulb" menu ( <kbd>Ctrl</kbd> + <kbd>.</kbd> ).

If cancellation is not a major concern for a project, the [analyzer severity](https://docs.microsoft.com/en-us/visualstudio/code-quality/use-roslyn-analyzers#configure-severity-levels) can be downgraded using an [.`editorconfig` file](https://editorconfig.org/):

```ini
[*.cs]
dotnet_diagnostic.NSB0002.severity = suggestion
```
