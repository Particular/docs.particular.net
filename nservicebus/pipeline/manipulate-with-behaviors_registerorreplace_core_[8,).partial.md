## Add or replace a step

The `Register` API will throw an exception when a behavior is registered with an ID that is already present in the pipeline. On the other hand, the `Replace` API will throw an exception when a behavior is registered with an ID that is _not_ found in the pipeline.

Sometimes it's impossible to know up front what the content of the pipeline is when registering a behavior.

To ensure the creation or replacement of the behavior in the pipeline, the `RegisterOrReplace`-API can be used.

snippet: RegisterOrReplaceStep
