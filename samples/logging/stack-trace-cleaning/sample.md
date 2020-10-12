---
title: Stack Trace Cleaning
summary: Shows how to minimize the stack trace written to the Error queue and the log output.
reviewed: 2019-07-22
component: Core
related:
- nservicebus/pipeline
- nservicebus/logging
- nservicebus/logging/nlog
- nservicebus/recoverability
- nservicebus/messaging/headers
---


## Introduction

This sample leverages the logging and recoverability APIs to remove some of the noise from exception information written to both the error queue and the and the log output. This is especially useful when dealing with async stack traces.

NOTE: .NET Core 2.1 [already makes async stack traces more readable](https://github.com/dotnet/corefx/issues/24627). This sample is only meant to be used on the .NET Framework or on .NET Core 2.0 and below.


## Solution Layout

The solution consists of two projects. `SampleWithoutClean` which takes the standard approach to converting an exception to a string. `SampleWithClean` which cleans the exception information before allowing it to be written.


### The Handler

The code in the handler throws an exception.

snippet: handler


### Retries are disabled

Retries are disabled so as to reduce the noise of the handler throwing exceptions multiple times.

snippet: disable-retries


## Before optimizations

Before the above optimizations the below (**45 lines and ~4900 characters**) is written to both the log and the error queue when the above handler throws.

```
System.Exception: Foo
at Handler.Handle(Message message, IMessageHandlerContext context) in C:\Code\docs.particular.net\samples\logging\stack-trace-cleaning\Core_6\SampleWithoutClean\Handler.cs:line 9
at NServiceBus.InvokeHandlerTerminator.<Terminate>d__0.MoveNext() in C:\Build\src\NServiceBus.Core\Pipeline\Incoming\InvokeHandlerTerminator.cs:line 19
--- End of stack trace from previous location where exception was thrown ---
at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
at NServiceBus.LoadHandlersConnector.<Invoke>d__1.MoveNext() in C:\Build\src\NServiceBus.Core\Pipeline\Incoming\LoadHandlersConnector.cs:line 41
--- End of stack trace from previous location where exception was thrown ---
at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
at NServiceBus.MutateIncomingMessageBehavior.<Invoke>d__0.MoveNext() in C:\Build\src\NServiceBus.Core\Pipeline\MutateInstanceMessage\MutateIncomingMessageBehavior.cs:line 28
--- End of stack trace from previous location where exception was thrown ---
at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
at NServiceBus.DeserializeLogicalMessagesConnector.<Invoke>d__1.MoveNext() in C:\Build\src\NServiceBus.Core\Pipeline\Incoming\DeserializeLogicalMessagesConnector.cs:line 30
--- End of stack trace from previous location where exception was thrown ---
at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
at NServiceBus.SubscriptionReceiverBehavior.<Invoke>d__1.MoveNext() in C:\Build\src\NServiceBus.Core\Routing\MessageDrivenSubscriptions\SubscriptionReceiverBehavior.cs:line 30
--- End of stack trace from previous location where exception was thrown ---
at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
at NServiceBus.MutateIncomingTransportMessageBehavior.<Invoke>d__0.MoveNext() in C:\Build\src\NServiceBus.Core\Pipeline\MutateTransportMessage\MutateIncomingTransportMessageBehavior.cs:line 27
--- End of stack trace from previous location where exception was thrown ---
at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
at NServiceBus.UnitOfWorkBehavior.<Invoke>d__0.MoveNext() in C:\Build\src\NServiceBus.Core\UnitOfWork\UnitOfWorkBehavior.cs:line 26
--- End of stack trace from previous location where exception was thrown ---
at NServiceBus.UnitOfWorkBehavior.<Invoke>d__0.MoveNext() in C:\Build\src\NServiceBus.Core\UnitOfWork\UnitOfWorkBehavior.cs:line 48
--- End of stack trace from previous location where exception was thrown ---
at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
at NServiceBus.ProcessingStatisticsBehavior.<Invoke>d__0.MoveNext() in C:\Build\src\NServiceBus.Core\Performance\Statistics\ProcessingStatisticsBehavior.cs:line 25
--- End of stack trace from previous location where exception was thrown ---
at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
at NServiceBus.ReceivePerformanceDiagnosticsBehavior.<Invoke>d__2.MoveNext() in C:\Build\src\NServiceBus.Core\Performance\Statistics\ReceivePerformanceDiagnosticsBehavior.cs:line 40
--- End of stack trace from previous location where exception was thrown ---
at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
at NServiceBus.TransportReceiveToPhysicalMessageProcessingConnector.<Invoke>d__1.MoveNext() in C:\Build\src\NServiceBus.Core\Pipeline\Incoming\TransportReceiveToPhysicalMessageProcessingConnector.cs:line 38
--- End of stack trace from previous location where exception was thrown ---
at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
at NServiceBus.MoveFaultsToErrorQueueBehavior.<Invoke>d__3.MoveNext() in C:\Build\src\NServiceBus.Core\Recoverability\Faults\MoveFaultsToErrorQueueBehavior.cs:line 38
```


## Manipulate Error Queue Header

NServiceBus has no explicit API to control what is written to the exceptions headers when messages are handled by [recoverability](/nservicebus/recoverability). Instead this sample leverages the [error message header customizations](/nservicebus/recoverability/configure-error-handling.md) to manipulate the headers after they are added, but before the error message is sent.


### The Stack Trace Cleaner

The cleaner uses some simple string manipulation to remove much of the noise from the exception information. It reads the information from the current header, and then overwrites that header with the result.

snippet: StackTraceCleaner

WARNING: To keep the sample simple no effort has been made to localize this. So for example `End of stack trace from...` may be different in other locals.


### Configuring the Error Header Customizations

The above cleaner is passed to Recoverability extension point.

snippet: customization-config


## Manipulate Logging Output

The [default logging](/nservicebus/logging/) included in NServiceBus does not support overwriting how exceptions are written to the log. So this sample uses NLog with a custom `LayoutRenderer` for exceptions.


### Layout Renderer

> Layout renderers are template macros that are used in Layouts.

See also: [NLog Layout-Renderers](https://github.com/nlog/nlog/wiki/Layout-Renderers).

This sample leverages the existing [Exception Layout Renderer]( https://github.com/nlog/nlog/wiki/Exception-Layout-Renderer) and override how exceptions are converted to strings.

This samples also uses the [AsyncFriendlyStackTrace Project](https://github.com/aelij/AsyncFriendlyStackTrace) so simplify the conversion logic. However other approaches, such as string manipulation, could also be used.

snippet: Renderer


### Configure Layout Renderer and NLog

The endpoint is then configured to the layout Renderer:

snippet: ConfigureNLog


## Result


### Logging with optimizations

With the above optimizations the following text (**14 lines and ~2100 characters**) is written to the log.

```
System.Exception: Foo
at Handler.Handle(Message message, IMessageHandlerContext context) in C:\Code\docs.particular.net\samples\logging\stack-trace-cleaning\Core_6\SampleWithClean\Handler.cs:line 10
at async NServiceBus.InvokeHandlerTerminator.Terminate(?) in C:\Build\src\NServiceBus.Core\Pipeline\Incoming\InvokeHandlerTerminator.cs:line 19
at async NServiceBus.LoadHandlersConnector.Invoke(?) in C:\Build\src\NServiceBus.Core\Pipeline\Incoming\LoadHandlersConnector.cs:line 41
at async NServiceBus.MutateIncomingMessageBehavior.Invoke(?) in C:\Build\src\NServiceBus.Core\Pipeline\MutateInstanceMessage\MutateIncomingMessageBehavior.cs:line 28
at async NServiceBus.DeserializeLogicalMessagesConnector.Invoke(?) in C:\Build\src\NServiceBus.Core\Pipeline\Incoming\DeserializeLogicalMessagesConnector.cs:line 30
at async NServiceBus.SubscriptionReceiverBehavior.Invoke(?) in C:\Build\src\NServiceBus.Core\Routing\MessageDrivenSubscriptions\SubscriptionReceiverBehavior.cs:line 30
at async NServiceBus.MutateIncomingTransportMessageBehavior.Invoke(?) in C:\Build\src\NServiceBus.Core\Pipeline\MutateTransportMessage\MutateIncomingTransportMessageBehavior.cs:line 27
at async NServiceBus.UnitOfWorkBehavior.Invoke(?) in C:\Build\src\NServiceBus.Core\UnitOfWork\UnitOfWorkBehavior.cs:line 26
at async NServiceBus.UnitOfWorkBehavior.Invoke(?) in C:\Build\src\NServiceBus.Core\UnitOfWork\UnitOfWorkBehavior.cs:line 48
at async NServiceBus.ProcessingStatisticsBehavior.Invoke(?) in C:\Build\src\NServiceBus.Core\Performance\Statistics\ProcessingStatisticsBehavior.cs:line 25
at async NServiceBus.ReceivePerformanceDiagnosticsBehavior.Invoke(?) in C:\Build\src\NServiceBus.Core\Performance\Statistics\ReceivePerformanceDiagnosticsBehavior.cs:line 40
at async NServiceBus.TransportReceiveToPhysicalMessageProcessingConnector.Invoke(?) in C:\Build\src\NServiceBus.Core\Pipeline\Incoming\TransportReceiveToPhysicalMessageProcessingConnector.cs:line 38
at async NServiceBus.MoveFaultsToErrorQueueBehavior.Invoke(?) in C:\Build\src\NServiceBus.Core\Recoverability\Faults\MoveFaultsToErrorQueueBehavior.cs:line 38
```


### Error Queue with optimizations

With the above optimizations the following text (**14 lines and ~2300 characters**) is written to the error queue.

```
System.Exception: Foo
at Handler.Handle(Message message, IMessageHandlerContext context) in C:\Code\docs.particular.net\samples\logging\stack-trace-cleaning\Core_6\SampleWithClean\Handler.cs:line 10
at NServiceBus.InvokeHandlerTerminator.&lt;Terminate&gt;d__0.MoveNext() in C:\Build\src\NServiceBus.Core\Pipeline\Incoming\InvokeHandlerTerminator.cs:line 19
at NServiceBus.LoadHandlersConnector.&lt;Invoke&gt;d__1.MoveNext() in C:\Build\src\NServiceBus.Core\Pipeline\Incoming\LoadHandlersConnector.cs:line 41
at NServiceBus.MutateIncomingMessageBehavior.&lt;Invoke&gt;d__0.MoveNext() in C:\Build\src\NServiceBus.Core\Pipeline\MutateInstanceMessage\MutateIncomingMessageBehavior.cs:line 28
at NServiceBus.DeserializeLogicalMessagesConnector.&lt;Invoke&gt;d__1.MoveNext() in C:\Build\src\NServiceBus.Core\Pipeline\Incoming\DeserializeLogicalMessagesConnector.cs:line 30
at NServiceBus.SubscriptionReceiverBehavior.&lt;Invoke&gt;d__1.MoveNext() in C:\Build\src\NServiceBus.Core\Routing\MessageDrivenSubscriptions\SubscriptionReceiverBehavior.cs:line 30
at NServiceBus.MutateIncomingTransportMessageBehavior.&lt;Invoke&gt;d__0.MoveNext() in C:\Build\src\NServiceBus.Core\Pipeline\MutateTransportMessage\MutateIncomingTransportMessageBehavior.cs:line 27
at NServiceBus.UnitOfWorkBehavior.&lt;Invoke&gt;d__0.MoveNext() in C:\Build\src\NServiceBus.Core\UnitOfWork\UnitOfWorkBehavior.cs:line 26
at NServiceBus.UnitOfWorkBehavior.&lt;Invoke&gt;d__0.MoveNext() in C:\Build\src\NServiceBus.Core\UnitOfWork\UnitOfWorkBehavior.cs:line 48
at NServiceBus.ProcessingStatisticsBehavior.&lt;Invoke&gt;d__0.MoveNext() in C:\Build\src\NServiceBus.Core\Performance\Statistics\ProcessingStatisticsBehavior.cs:line 25
at NServiceBus.ReceivePerformanceDiagnosticsBehavior.&lt;Invoke&gt;d__2.MoveNext() in C:\Build\src\NServiceBus.Core\Performance\Statistics\ReceivePerformanceDiagnosticsBehavior.cs:line 40
at NServiceBus.TransportReceiveToPhysicalMessageProcessingConnector.&lt;Invoke&gt;d__1.MoveNext() in C:\Build\src\NServiceBus.Core\Pipeline\Incoming\TransportReceiveToPhysicalMessageProcessingConnector.cs:line 38
at NServiceBus.MoveFaultsToErrorQueueBehavior.&lt;Invoke&gt;d__3.MoveNext() in C:\Build\src\NServiceBus.Core\Recoverability\Faults\MoveFaultsToErrorQueueBehavior.cs:line 38
```