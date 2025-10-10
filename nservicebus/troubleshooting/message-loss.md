---
title: Troubleshooting message loss
summary: Troubleshooting message loss causes when coding against the NServiceBus API
reviewed: 2025-10-09
component: core
---
In most cases, the cause of message loss is one of the following scenarios:

- Missing `await` keyword on async methods
- Using `async void` in the call stack
- Catching (generic) exceptions but not rethrowing the exception

All of these will prevent the recovery mechanism in NServiceBus from working correctly, as exceptions will not be caught by NServiceBus. This results in NServiceBus assuming processing was successful when it was not. Another side effect of the async-related issues is that transactions can be committed too early, which can cause corruption in storage.

Consider enabling [message auditing](/nservicebus/operations/auditing.md) so that the message is not lost.

## Missing await keyword

Although all async methods that are provided in Particular APIs do not have the `Async` suffix, these methods **must** be awaited. All async methods must be awaited, including async methods from other libraries.

You can resolve this by:

- Adding the `await` keyword everywhere in the call stack.
- Enabling the complier warnings as error. Most of the time, these issues are highlighted by the compiler as compiler warnings. So you can enable "warn as error" setting.
- Appending with `.GetAwaiter().GetResult()` if await cannot be used. This is considered a workaround, and it's recommended to make the call stack fully asynchronous to prevent potential deadlocks.

## Usage of async void

In the following example, using `async void` (MyAsyncVoidMethod()) is very similar in behavior to missing await (MyMethod()). This is often done to work around the fact that the method calls async methods, but the function invoking it does not have a valid signature.

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
Also, see [Missing await keyword](#missing-await-keyword) above.

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

- Removing the try-catch block (or ensure exceptions are rethrown) so that any failures bubble up and are observed by NServiceBus. This allows the recoverability pipeline to trigger retries or move the message to the error queue.
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
