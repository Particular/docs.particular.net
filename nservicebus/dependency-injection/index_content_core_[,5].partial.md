### Endpoint resolution

Note that the instance of `IBus` is scoped for the lifetime of the container. Hence to resolve `IBus` and then dispose of it the endpoint will stop processing messages. Note that all NServiceBus services, including `IBus`, will be injected into the passed in dependency injection instance. As such there is no need to register these instances at configuration time. 


### Cleanup

When using an external container, the bus instance is not automatically disposed. To dispose of the resources properly:

 1. Call IBus.Dispose() to dispose of the bus.
 1. And then call container.Dispose() to dispose of the container.

NOTE: When external dependency injection is not used, the bus instance still needs to be properly disposed of by calling `IBus.Dispose()`.
