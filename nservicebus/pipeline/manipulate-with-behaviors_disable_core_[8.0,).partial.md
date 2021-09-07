## Disable an existing step

To disable the implementation of an existing step, substitute it with a no action behavior:

snippet: NoActionPipelineBehavior

The behaviour does nothing and calls the next step in the pipeline chain by invoking `next()`.

snippet: NoActionPipelineStep
