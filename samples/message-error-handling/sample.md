---
title: Custom Exception Handling
summary: With custom exception handling it is possible to fine tune how the exceptions should be handled after they have automatically been retried
reviewed: 2016-10-26
component: Core
tags:
 - Error Handling
 - Exceptions
 - Retry
related:
 - nservicebus/recoverability
 - nservicebus/pipeline/customizing-error-handling
---

partial: usage

This sample shows how based on the exception type a message can be retried, sent to the error queue or ignored. Beware of swallowing the exceptions though, since it is almost never intended and the message will be removed from the queue, as if it has been processed successfully. 

snippet: MoveToErrorQueue

To register the new exception handler it needs to plugged in:

snippet: registering-behavior
