## Via a pipeline Behavior

The pipeline can also be aborted by injecting a custom Behavior that, with some custom logic, optionally decides to abort Behaviors nested inside it.

snippet: AbortPipelineWithBehavior

For more information about creating and where to inject a behavior see [customizing the pipeline](/nservicebus/pipeline/manipulate-with-behaviors.md).