
### Endpoint resolution

Note that all NServiceBus services will be injected into the passed in dependency injection instance. As such there is no need to register these instances at configuration time. 

Note: `IEndpointInstance` must be registered to be properly resolved.


### Cleanup

NOTE: In NServiceBus version 6 and above `IEndpointInstance` is not `IDisposable`.
