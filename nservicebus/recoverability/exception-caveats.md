---
title: Exception caveats
summary: NServiceBus cannot guarantee the handling of certain types of exceptions.
component: core
reviewed: 2020-09-16
redirects:
 - nservicebus/errors/exception-caveats
---

NServiceBus cannot guarantee the handling of certain types of exceptions.


## `AccessViolationException`

If an [`AccessViolationException`](https://docs.microsoft.com/en-us/dotnet/api/system.accessviolationexception) is thrown, the endpoint is likely to terminate. This is because an `AccessViolationException` thrown by the common language runtime cannot be caught by a `try...catch` block.

While this problem can be mitigated by using [`HandleProcessCorruptedStateExceptionsAttribute`](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.exceptionservices.handleprocesscorruptedstateexceptionsattribute), Microsoft explicitly recommends not to do this.

> Corrupted process state exceptions are exceptions that indicate that the state of a process has been corrupted.

For more information see [_Handling Corrupted State Exceptions_](https://msdn.microsoft.com/en-us/magazine/dd419661.aspx#id0070035).


## `StackOverflowException`

If a [`StackOverflowException`](https://docs.microsoft.com/en-us/dotnet/api/system.stackoverflowexception) is thrown, the process will terminate because the exception cannot be caught by a `try...catch` block.


## `OutOfMemoryException`

If an [`OutOfMemoryException`](https://docs.microsoft.com/en-us/dotnet/api/system.outofmemoryexception) is thrown, the process may terminate. This is because, even though this exception type is handled in the same way as all others, if the lack of sufficient memory persists, another instance of `OutOfMemoryException` may be thrown while the original exception is being handled.
