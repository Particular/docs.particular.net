---
title: Aborting Pipeline execution
summary: How to tell NServiceBus to abort processing any more handlers in the pipeline
component: Core
reviewed: 2017-03-30
tags:
- Pipeline
- Security
---


## From inside a Handler

From the context of a Handler further Handler execution can be aborted by calling the `DoNotContinueDispatchingCurrentMessageToHandlers()` method. This method instruct the bus not to pass the current message on to subsequent handlers in the pipeline. This is often used by authentication and authorization handlers when those checks fail.

snippet: AbortHandler

Aborting the pipeline does not fail the message processing. The message that was processed will be marked as successfully completed.

Warning: Handler execution order by default is non-deterministic. To configure the ordering see [Handler Ordering](/nservicebus/handlers/handler-ordering.md).


partial: behavior