---
title: Cancellation and catching exceptions
summary: How to correctly catch exceptions from cancellable operations
reviewed: 2021-05-26
related:
  - nservicebus/hosting/cooperative-cancellation
---

When catching exceptions from cancellable operations, a distinction should be made between exceptions thrown due to cancellation, and exceptions thrown for other reasons.

A cancellable operation is one that is passed a [`CancellationToken`](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) as an argument. For example:

```csharp
await foo.Bar(cancellationToken).ConfigureAwait(false);
```

An exception thrown by this operation only represents _cancellation_ when its type inherits from [`OperationCanceledException`](https://docs.microsoft.com/en-us/dotnet/api/system.operationcanceledexception) and when the [`CancellationToken.IsCancellationRequested`](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.iscancellationrequested) property is `true`.  Conversely, the exception does _not_ represent cancellation when its type does not inherit from `OperationCanceledException` or when the `CancellationToken.IsCancellationRequested` property is `false`.

Note that an `OperationCanceledException` thrown when `CancellationToken.IsCancellationRequested` is `false`, does _not_ represent cancellation. All this means is that `foo.Bar` threw an `OperationCanceledException` for some reason other than cancellation, and the operation should be treated as a failure.

## Catching `System.Exception`

Most of the time, when `System.Exception` is caught, the assumption is that the operation has failed, not that it has been canceled. In these cases, the correct way to catch the `Exception` is add a filter which excludes exceptions which represent cancellation:

```csharp
try
{
    await foo.Bar(cancellationToken).ConfigureAwait(false);
}
catch (Exception ex) when (ex is not OperationCanceledException || !cancellationToken.IsCancellationRequested)
{
    // foo.Bar failed — take appropriate action, including re-throwing the exception if appropriate
}
```

Note that, in the above example, exceptions that represent cancellation are _not_ caught. This is desirable because cancellation should be propagated to the caller of the current method.

## Catching `System.OperationCanceledException`

In most cases, exceptions which represent cancellation should not be caught, and should be allowed to propagate to the caller of the current method. In some cases, it may be necessary to catch these exceptions to take specific actions. The correct way to catch these exceptions is to add a filter which includes only exceptions which represent cancellation:

```csharp
try
{
    await foo.Bar(cancellationToken).ConfigureAwait(false);
}
catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
{
    // foo.Bar was cancelled — take appropriate action

    // re-throw the exception to propagate the cancellation to the caller of the current method
    throw;
}
catch (Exception ex)
{
    // this catch (if it is required) will now catch only exceptions which do NOT 
    // represent cancellation, so it does not require a filter
}
```

## Helper methods

If exception handling is widespread, it may be helpful to introduce an `IsCausedBy` extension method:

```csharp
public static bool IsCausedBy(this Exception ex, CancellationToken cancellationToken) =>
    ex is OperationCanceledException && cancellationToken.IsCancellationRequested;
```

Using this method, the `catch` filters are much simpler in both cases:

```csharp
try
{
    await foo.Bar(cancellationToken).ConfigureAwait(false);
}
catch (Exception ex) when (!ex.IsCausedBy(cancellationToken))
{
    // foo.Bar failed — take appropriate action, including re-throwing the exception if appropriate
}
```

```csharp
try
{
    await foo.Bar(cancellationToken).ConfigureAwait(false);
}
catch (Exception ex) when (ex.IsCausedBy(cancellationToken))
{
    // foo.Bar was cancelled — take appropriate action

    // re-throw the exception to propagate the cancellation to the caller of the current method
    throw;
}
```

Note that the second example is catching `Exception` rather than `OperationCanceledException` and the `IsCausedBy` method is filtering on the exception type instead of the `catch` clause itself. This results in one extra type comparison (`isinst`) in the resulting IL code, but the performance cost is negligible and the code is simpler to read.

## Inside the message processing pipeline

For code inside the message processing pipeline, such as a message handler, saga, or pipeline behavior, the above considerations are the same. The only difference is that the `CancellationToken` is provided by the `context.CancellationToken` property.

However, it is generally preferred to not catch exceptions within message handlers and sagas, and instead let exceptions be handled by [recoverability](/nservicebus/recoverability/).
