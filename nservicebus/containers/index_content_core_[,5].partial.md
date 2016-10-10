### Endpoint resolution

Note that the instance of `IBus` is scoped for the lifetime of the container. Hence to resolve `IBus` and then dispose of it the endpoint will stop processing messages. Note that all NServiceBus services, including `IBus`, will be injected into the passed in container instance. As such there is no need to register these instances at configuration time. 


### Cleanup

When using an external container normally the bus instance is not disposed of automatically. The following dispose sequence should be applied if latest container adapter packages are used:

1. Dispose the bus by calling `IBus.Dispose()`.
2. Dispose the external container with `container.Dispose()`.
