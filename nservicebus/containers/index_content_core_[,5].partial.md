### Endpoint resolution

Note that the instance of `IBus` is scoped for the lifetime of the container. Hence to resolve `IBus` and then dispose of it the endpoint will stop processing messages. Note that all NServiceBus services, including `IBus`, will be injected into the passed in container instance. As such there is no need to register these instances at configuration time. 


### Cleanup

When using an external container normally the bus instance is not disposed of manually. If `IBus.Dispose()` is called that would indirectly trigger the container to dispose lifetime scope.

NOTE: Do NOT call `IBus.Dispose` when using an external container, instead call dispose in the container during shutdown.
