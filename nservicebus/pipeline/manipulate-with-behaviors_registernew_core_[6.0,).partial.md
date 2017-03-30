
To add a custom behavior to the pipeline, register it from the endpoint configuration:

snippet: RegisterBehaviorEndpointConfiguration

Behaviors can also be registered from a `Feature` as shown below:

snippet: RegisterBehaviorFromFeature

WARNING: Behaviors are only created once and the same instance is reused on every invocation of the pipeline. Consequently, every behavior dependency will also behave as a singleton, even if a different option was specified when registering it in the DI container. Furthermore, the behavior as well as the dependencies called during the invocation phase needs to be concurrency safe.
