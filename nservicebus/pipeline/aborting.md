---
title: Aborting Pipeline execution
summary: How to tell NServiceBus to abort processing any more handlers in the pipeline
component: Core
reviewed: 2025-11-24
---

NServiceBus allows for aborting handlers execution. This can be done either from within a message handler or by using a custom pipeline behavior.

## From inside a Handler

If the endppoint hosts more than one handler for the same message type, from the context of an handler, further handler execution can be aborted by calling the `DoNotContinueDispatchingCurrentMessageToHandlers()` method. This method instructs the NServiceBus not to pass the current message on to subsequent handlers in the pipeline. This is often used by authentication and authorization handlers when those checks fail.

snippet: AbortHandler

> [!IMPORTANT]
> Aborting the pipeline does not fail the message processing. The message will be marked as successfully completed.

> [!WARNING]
> Handler execution order is non-deterministic by default. To configure ordering see [Handler Ordering](/nservicebus/handlers/handler-ordering.md).

## Via a pipeline Behavior

The pipeline can also be aborted by injecting a custom Behavior that, with some custom logic, optionally decides to abort any nested behaviors.

snippet: AbortPipelineWithBehavior

For more information about creating and injecting a behavior, see [customizing the pipeline](/nservicebus/pipeline/manipulate-with-behaviors.md).
