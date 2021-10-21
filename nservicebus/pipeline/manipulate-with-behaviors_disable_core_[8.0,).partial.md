## Disable an existing step

To disable the implementation of an existing step, substitute it with a behavior with no action:

snippet: NoActionPipelineBehavior

The behavior does nothing and calls the next step in the pipeline chain by invoking `next()`.

snippet: NoActionPipelineStep
