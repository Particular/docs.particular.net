---
title: Cancellation and exception handling
summary: Using Azure for endpoint hosting and to provide Transports and Persistence
reviewed: 2021-05-26
---

In most cases, code in NServiceBus handlers and sagas should not catch exceptions, and prefer instead allowing [recoverability](/nservicebus/recoverability/) to deal with exceptions using retries.

However, when it is necessary to handle exceptions, care should be taken so that [cooperative cancellation](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation) will work correctly when the endpoint needs to be shut down.

All NServiceBus components use the [Particular.Analyzers](https://www.myget.org/feed/particular/package/nuget/Particular.Analyzers) package which contains Roslyn analyzers to verify the patterns described here, among others. If developing an extension to NServiceBus, using the same analyzers package is highly recommended.

## Catching `Exception`

One common pattern is to catch and swallow the general `Exception` type in some cases, like this:

```csharp
try
{
    await SomeOperation(cancellationToken);
}
catch (Exception ex) // BAD
{
    log.Warn("Something bad happened.", ex);
}
```

However, because the `cancellationToken` is being passed to `SomeOperation`, the code inside the `try` block may generate an `OperationCanceledException` when the host infrastructure needs the endpoint to shut down. Because `OperationCanceledException` inherits from the general `Exception` type, cancellation will also fall into the `catch` block, and cancellation will be interrupted.

## Catching `OperationCanceledException`

The previous pattern can be improved by first catching `OperationCanceledException`:

```csharp
try
{
    await SomeOperation(cancellationToken);
}
catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
{
    throw;
}
catch (Exception ex)
{
    log.Warn("Something bad happened.", ex);
}
```

In this case, an `OperationCanceledException` that occurs when the endpoint is shutting down will be thrown so that cancellation can continue correctly. It's important to add the exception filter `when (cancellationToken.IsCancellationRequested)` so that it's clear that the exception is happening because the endpoint is shutting down.

## Without catching `OperationCanceledException`

It's also possible to combine `catch` blocks using a filter that takes the exception type into account:

```csharp
try
{
    await SomeOperation(cancellationToken);
}
catch (Exception ex) when (ex is not OperationCanceledException)
{
    log.Warn("Something bad happened.", ex);
}
```

## Helper methods

If exception handling is widespread, it may be possible to introduce an `IsCausedBy` extension method:

```csharp
public static bool IsCausedBy(this Exception ex, CancellationToken cancellationToken) =>
    ex is OperationCanceledException && cancellationToken.IsCancellationRequested;
```

This allows simplifying `catch` filters on the general `Exception` type:

```csharp
// To warn that shutdown is occurring and then throw the OperationCanceledException
catch (Exception ex) when (ex.IsCausedBy(cancellationToken))

// To swallow all exceptions except an OperationCanceledException during shutdown
catch (Exception ex) when (!ex.IsCausedBy(cancellationToken))
```
