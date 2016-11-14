
To add a custom behavior to the pipeline, register it from the endpoint configuration:

snippet:RegisterBehaviorEndpointConfiguration

Behaviors can also be registered from a `Feature` as shown below:

snippet:RegisterBehaviorFromFeature

WARNING: Behaviors are instantiated once and that instance is then reused on every invocation of the pipeline, even when registering the behavior to be created by a Dependency Injection Container. Never use dependency injection for dependencies configured with an *InstancePerUnitOfWork* scope.
