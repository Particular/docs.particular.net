---
title: Custom Exception Handling
summary: With custom exception handling, it is possible to fine-tune how exceptions should be handled after they have been retried
reviewed: 2020-12-22
component: Core
related:
 - nservicebus/recoverability
 - nservicebus/pipeline/customizing-error-handling
---

This sample shows how, based on the exception type, a message can be either retried, sent to the error queue, or ignored. The portable Particular Service Platform will list the messages arriving in the error queue.

include: platformlauncher-windows-required

partial: usage

snippet: MoveToErrorQueue

To register the new exception handler:

snippet: registering-behavior

Beware of swallowing exceptions though, since it is almost never intended and the message will be removed from the queue, as if it has been processed successfully.
