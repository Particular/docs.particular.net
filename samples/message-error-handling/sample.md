---
title: Custom Exception Handling
summary: With custom exception handling, it is possible to fine-tune how exceptions should be handled after they have been retried
reviewed: 2024-02-28
component: Core
related:
 - nservicebus/recoverability
 - nservicebus/pipeline/customizing-error-handling
---

This sample shows how a message can be either retried, sent to the error queue, or ignored based on the exception type. The portable Particular Service Platform will list the messages arriving in the error queue.

include: platformlauncher-windows-required

In Versions 6 and above, the `IManageMessageFailures` is deprecated, and there's no direct way to manage custom exceptions. The Recoverability API allows for a much easier configuration of immediate and delayed retries. However, finer-grain control can be achieved by writing a custom Behavior and executing it as a step in the message-handling pipeline.

snippet: MoveToErrorQueue

To register the new exception handler:

snippet: registering-behavior

Beware of swallowing exceptions, though, since it is rarely intended, and the message will be removed from the input queue as if it has been processed successfully.
