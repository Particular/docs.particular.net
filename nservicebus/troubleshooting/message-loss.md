---
title: Troubleshooting message loss
summary: NServiceBus troubleshooting message loss
reviewed: 2020-10-14
component: core
---
In most cases the cause of message loss is one of the following scenarios:

- Missing `await` keyword on async methods
- Using `async void` in the call stack
- Catching (generic) exceptions but not rethrowing the exception

All of these will prevent the recovery mechanism in NServiceBus from working correctly as exceptions will not be caught by NServiceBus. This results in NServiceBus assuming processing was succesful when it was not. Another side effect of the async related issues is that transactions can be committed too early which can cause corruption in storage.

Consider enabling [message auditing](/nservicebus/operations/auditing.md) so that the message is not lost.

## Missing await keyword

Although all async methods that are provided in Particular APIs do not have the `Async` suffix these methods **must** be awaited. All async methods must be awaited, including async methods from other libraries.

Resolve this by:

- Adding the `await` keyword everywhere in the call stack.
- Most of the time, these issues are highlighted by the compiler as compiler warning; consider enabling "warn as error".
- If await cannot be used, append `.GetAwaiter().GetResult()`. This is considered a workaround and it's recommend to make the call stack fully asynchronous to prevent potential deadlocks


## Usage of async void

Using `async void` is very similar in behavior to missing await. This is often done to work around the fact that the method calls async methods but the function invoking it does not have a valid signature.

```c#
void MyMethod()
{
    MyAsyncMethod();
}

async void MyAsyncVoidMethod();
{
    await MyMethodAsync();
}

async Task MyMethodAsync()
{
    // async stuff
}
```

Resolve this by:

- Changing the signature to `async Task` and `await` the call in the method that is invoking this method. Also see [Missing await keyword](#missing-await-keyword) above.


## Catching exceptions

The following is often observed:

```
try
{
    // Code that throws an exception is here
}
catch (Exception e)
{
}
```

The problem here is that the exception is caught and discarded. Therefore NServiceBus will never receive this exception in its pipeline. From the perspective of NServiceBus, eveything is fine. The message will not be retried by the recovery mechanism.

Resolve this by:

- Completely remove the try-catch
- Use the `throw` keyword in the `catch`. Do *not* use `throw e` as this hides exception information.
- Use `throw new Exception("My detailed reason for this exception with the exception as inner exception", e)`

