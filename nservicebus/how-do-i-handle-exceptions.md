---
title: How to Handle Exceptions
summary: Don't try to handle exceptions in your message handlers. Let NServiceBus do it for you.
tags:
- Exceptions
- Error Handling
- Automatic Retries
---

Don't.

NServiceBus has exception catching and handling logic of its own which surrounds all calls to user code. When an exception bubbles through to the NServiceBus infrastructure, it rolls back the transaction on a transactional endpoint, causing the message to be returned to the queue, and any messages that the user code tried to send or publish to be undone as well.

At that point, NServiceBus retries to handle that message a configurable number of times (default of 5) and if the message fails on every one of those retries, the message is then moved to the configured error queue. For details, see discussion on [Second-level Retries](/nservicebus/second-level-retries.md).

Administrators should monitor that error queue so that they can see when problems occur. The message in the error queue contains the source queue and machine so that the administrator can see what's wrong with that node and possibly correct the problem (like bringing up a database that went down).

Monitoring and handling of failed messages with [ServicePulse](/servicepulse) provides access to full exception details (including stacktrace, and throught ServiceInsight it also enables advanced debugging with all message context. It also provides a manual "retry" option (i.e. send the message for re-processing). for more details, see [Introduction to Failed Messages Monitoring in ServicePulse](/servicepulse/intro-failed-messages.md). 

If ServicePulse or ServiceInsight are not available in your environment, you can use of the  `ReturnToSourceQueue.exe` tool to send the relevant message back to its original queue so that it can be processed again. The `ReturnToSourceQueue` tool is specific to MSMQ, and can be found in the [NServiceBus GitHub repository](https://github.com/Particular/NServiceBus).

For more information on this process, [Transactions Message Processing](transactions-message-processing.md).

### Some caveats

Certain types of exceptions are special in their behavior and may require custom handling. 

#### AccessViolationException

If an [AccessViolationException](http://msdn.microsoft.com/en-us/library/system.accessviolationexception.aspx) is thrown then the endpoint will terminate. The reason is that a standard `try catch`, which NServiceBus uses does not catch an  `AccessViolationException` as such it will bubble out of he handler and terminate the endpoint.

While you can explicitly handle these exceptions (using a [HandleProcessCorruptedStateExceptionsAttribute](http://msdn.microsoft.com/en-us/library/system.runtime.exceptionservices.handleprocesscorruptedstateexceptionsattribute.aspx)) it is explicitly recommended MS not to do it. 

> Corrupted process state exceptions are exceptions that indicate that the state of a process has been corrupted. We do not recommend executing your application in this state.

For more information see [Handling Corrupted State Exceptions](http://msdn.microsoft.com/en-us/magazine/dd419661.aspx#id0070035)
 
#### StackOverflowException

NServiceBus can't handle [StackOverflowExceptions](http://msdn.microsoft.com/en-us/library/system.stackoverflowexception.aspx) since .net does not allow it.

> A StackOverflowException object cannot be caught by a try-catch block and the corresponding process is terminated by default. Consequently, users are advised to write their code to detect and prevent a stack overflow. For example, if your application depends on recursion, use a counter or a state condition to terminate the recursive loop. Note that an application that hosts the common language runtime (CLR) can specify that the CLR unload the application domain where the stack overflow exception occurs and let the corresponding process continue.

#### OutOfMemoryException

While [OutOfMemoryException](http://msdn.microsoft.com/en-us/library/system.outofmemoryexception.aspx) will be caught by NServiceBus and handled in the standard NServiceBus manner, there is no guarantee that there will be enough memory available to handle the exception appropriately. 
