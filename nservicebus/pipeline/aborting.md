---
title: Aborting Pipeline execution
summary: How to tell NServiceBus to abort processing any more handlers in the pipeline 
tags:
- Pipeline
---

## From inside a Handler

From the context of a Handler further Handler execution can be aborted by calling the `DoNotContinueDispatchingCurrentMessageToHandlers()` method. This method instruct the bus not to pass the current message on to subsequent handlers in the pipeline. This is often used by authentication and authorization handlers when those checks fail.

snippet: AbortHandler

Warning: Handler execution order by default is non-deterministic. To configure the ordering see [Handler Ordering](/nservicebus/handlers/handler-ordering.md).


## Via a pipeline Behavior

The pipeline can also be aborted by injecting a custom Behavior that, with some custom logic, optionally decides to abort Behaviors nested inside it.

snippet: AbortPipelineWithBehavior

For more information about creating and where to inject a behavior see [customizing the pipeline](/nservicebus/pipeline/manipulate-with-behaviors.md).