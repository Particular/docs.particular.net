---
title: Troubleshooting message loss
summary: Troubleshooting message loss scenarios when coding against the NServiceBus API
reviewed: 2025-10-21
component: core
---
In most cases, message loss can be traced to one of the following:

- Missing `await` keyword on async methods
- Using `async void` in the call stack
- Catching (generic) exceptions but not rethrowing the exception

All of these will prevent the recovery mechanism in NServiceBus from working correctly, as exceptions will not be caught by NServiceBus. This results in NServiceBus assuming processing was successful when it was not. In addition, async-related issues can cause transactions to be committed too early, which can result in data corruption.

Consider enabling [message auditing](/nservicebus/operations/auditing.md) so that the message is not lost.

## Missing await keyword

Although async methods provided in Particular APIs do not have the `async` suffix, these methods **must** be awaited. Always use `await` for async methods, including those from external libraries, at every level of the call stack. 

You can find missing `await` keywords by reviewing the compiler warnings for the project. Alternatively, enabling the [`WarningsAsErrors`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/errors-warnings) setting will ensure the project won’t compile if there are missing `await` keywords. And if `await` cannot be used, then appending the call with `.GetAwaiter().GetResult()` is an option. However, this is considered a workaround, and it’s recommended to make the call stack fully asynchronous to prevent potential deadlocks.

## Usage of async void

In the following example, using `async void` (MyAsyncVoidMethod()) has a similar outcome as [missing the `await` keyword](#missing-await-keyword) (MyMethod()). This is often done to work around the fact that the method calls async methods, but the function invoking it does not have a valid signature.

```c#
void MyMethod()
{
    MyAsyncMethod();
}

async void MyAsyncVoidMethod()
{
    await MyMethodAsync();
}

async Task MyMethodAsync()
{
    // async stuff
}
```

You can resolve this by changing the signature to `async Task` and `await` the call in the method that is invoking this method. 

```c#
async Task MyMethod()
{
    await MyAsyncVoidMethod();
}

async Task MyAsyncVoidMethod()
{
    await MyMethodAsync();
}

async Task MyMethodAsync()
{
    // async stuff
}
```

## Catching exceptions

In the following example, the exception is caught in the catch block but is discarded.

```c#
try
{
    // Code that throws an exception is here
}
catch (Exception e)
{
    //exception is caught but discarded
}
```

The problem here is that NServiceBus will never receive this exception in its pipeline. From the NServiceBus perspective, everything is fine. The message will not be retried by the recovery mechanism.

You can resolve this by:

- Removing the try-catch block (or ensure exceptions are rethrown) so that any failures bubble to NServiceBus. This allows the recoverability pipeline to trigger retries or move the message to the error queue.
- Using the `throw` keyword in the `catch`. Do *not* use `throw e` as this hides some original exception details.
- Using `throw new Exception("My detailed reason for this exception with the original exception as inner exception", e)`.
```c#
try
{
    // Code that throws an exception is here
}
catch (Exception e)
{
    throw new Exception("My detailed reason for this exception with the original exception as inner exception", e)
}
```
