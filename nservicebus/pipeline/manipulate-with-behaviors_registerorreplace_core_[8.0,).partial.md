## Add or replace a step

The `Register` API will fail when a behavior is registered that is already present in the pipeline.
On the counter side, the `Replace` API will fail when a behavior is registered that is not found in the pipeline.

Sometimes, it's impossible to know upfront what the content of the pipeline is when registering a behavior.

To ensure the creation or replacement of the behavior in the pipeline, the `RegisterOrReplace`-API can be used.

snippet: RegisterOrReplaceStep
