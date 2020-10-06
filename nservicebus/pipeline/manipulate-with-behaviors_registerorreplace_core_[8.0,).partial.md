## Add or replace a step

The `Register` API will throw when a behavior is registered with an ID that that is already present in the pipeline.
On the counter side, the `Replace` API will throw when a behavior is registered with an ID that is not found in the pipeline.

Sometimes, it's impossible to know upfront what the content of the pipeline is when registering a behavior.

To ensure the creation or replacement of the behavior in the pipeline, the `RegisterOrReplace`-API can be used.

snippet: RegisterOrReplaceStep
