
To add a custom behavior to the pipeline, register it from the endpoint configuration:

snippet: RegisterBehaviorEndpointConfiguration

Behaviors can also be registered from a `Feature` as shown below:

snippet: RegisterBehaviorFromFeature

WARNING: Behaviors are only created once and the same instance is reused on every invocation of the pipeline. Consequently, every behavior dependency will also behave as a singleton, even if a different option was specified when registering it in [dependency injection](/nservicebus/dependency-injection/). Furthermore, the behavior, and all dependencies called during the invocation phase, need to be concurrency safe.
