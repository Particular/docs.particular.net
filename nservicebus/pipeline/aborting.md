---
title: Aborting Pipeline execution
summary: How to tell NServiceBus to abort processing any more handlers in the pipeline
component: Core
reviewed: 2020-11-26
---


## From inside a Handler

From the context of a Handler, further handler execution can be aborted by calling the `DoNotContinueDispatchingCurrentMessageToHandlers()` method. This method instructs the bus not to pass the current message on to subsequent handlers in the pipeline. This is often used by authentication and authorization handlers when those checks fail.

snippet: AbortHandler

Aborting the pipeline does not fail the message processing. The message that was processed will be marked as successfully completed.

Warning: Handler execution order is non-deterministic by default. To configure the ordering see [Handler Ordering](/nservicebus/handlers/handler-ordering.md).


## Via a pipeline Behavior

The pipeline can also be aborted by injecting a custom Behavior that, with some custom logic, optionally decides to abort any nested behaviors.

snippet: AbortPipelineWithBehavior

For more information about creating and injecting a behavior, see [customizing the pipeline](/nservicebus/pipeline/manipulate-with-behaviors.md).
