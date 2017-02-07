---
title: Exception Caveats
summary: Certain types of exceptions cannot be handled nativity by NServiceBus.
component: core
reviewed: 2016-11-05
tags:
 - Exceptions
 - Error Handling
redirects:
 - nservicebus/errors/exception-caveats
---

Certain types of exceptions are special in their behavior and may require custom handling.


## AccessViolationException

If an [AccessViolationException](https://msdn.microsoft.com/en-us/library/system.accessviolationexception.aspx) is thrown then the endpoint will terminate. The reason is that a standard `try catch`, which NServiceBus uses does not catch an  `AccessViolationException` as such it will bubble out of he handler and terminate the endpoint.

While these exceptions can be explicitly handled (using a [HandleProcessCorruptedStateExceptionsAttribute](https://msdn.microsoft.com/en-us/library/system.runtime.exceptionservices.handleprocesscorruptedstateexceptionsattribute.aspx)) it is explicitly recommended MS not to do it.

> Corrupted process state exceptions are exceptions that indicate that the state of a process has been corrupted.

For more information see [Handling Corrupted State Exceptions](https://msdn.microsoft.com/en-us/magazine/dd419661.aspx#id0070035)


## StackOverflowException

[StackOverflowExceptions](https://msdn.microsoft.com/en-us/library/system.stackoverflowexception.aspx) cannot be handled since .NET does not allow it.

> A StackOverflowException object cannot be caught by a try-catch block and the corresponding process is terminated by default. Consequently, users are advised to write their code to detect and prevent a stack overflow. For example, if the application depends on recursion, use a counter or a state condition to terminate the recursive loop. Note that an application that hosts the common language runtime (CLR) can specify that the CLR unload the application domain where the stack overflow exception occurs and let the corresponding process continue.


## OutOfMemoryException

[OutOfMemoryException](https://msdn.microsoft.com/en-us/library/system.outofmemoryexception.aspx) will be handled in a similar manner as other exceptions. A message will be retried according to the endpoint configuration. However, if there isn't sufficient memory, then the process might crash.
