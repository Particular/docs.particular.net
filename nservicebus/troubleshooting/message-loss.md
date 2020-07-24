---
title: Troubleshooting message loss
summary: NServiceBus troubleshooting message loss
reviewed: 2020-07-24
component: NServiceBus
---
# Troubleshooting message loss

Sometimes customers are experiencing message loss scenarios. In most cases the cause is in any of the following scenarios:

- Missing `await` keyword on async methods
- Usage of `async void` in the call stack
- Catching (generic) exceptions but not rethrowing the exception

All these will prevent NServiceBus its recovery mechanism to work correctly as any exception will never be catched by NServiceBus. This results in NServiceBus to think processing was succesful while it (potentially) was not. Another side effect of the async related issues it that transactions can be committed too early which can cause corruption in your storage.

Consider enabling [message auditing](https://docs.particular.net/nservicebus/operations/auditing) so that atleast the message is not lost.

## Missing await keyword

Although all async methods that are provided in our API's do not have the `Async` suffix these methods **must** be awaited. All async methods must be awaited, including async methods from other libraries.

Resolve this by:

- Adding the `await` keyword everywhere in the call stack.
- Most of the time these issues are highlighted by the compiler as compiler warning, consider enabling "warn as error".
- If await cannot be used append `.GetAwaiter().GetResult()` but consider to fully make the call stack async to prevent potential deadlocks


## Usage of async void

Making of use `async void` is very similar in behavior to missing await. This is often done to workaround the fact that the method calls async methods but function invoking it does not have a valid signature.

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

- Changing the signature to `async Task` and `await` this in the method that is invoking this method. Also see [Missing await keyword](#missing-await-keyword).


## Catching exceptions

The following is often observed:

```
try
{
    // Code that throws an exception is here
}
catch(Exception e)
{
}
```

The problem is that the exception is catched. This results in NServiceBus never receiving this exception in its pipeline. From the perspective of NServiceBus eveything is fine. The message will not be retried by the recovery mechanism.

Resolve this by:

- Completely remove the try-catch
- Use the `throw` keyword in the catch (do not use `throw e` as that hides exception information)
- Use `throw new Exception("My detailed reason for this exception with the exception as inner exception", e)`

