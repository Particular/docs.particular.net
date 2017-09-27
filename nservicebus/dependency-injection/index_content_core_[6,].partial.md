
### Endpoint resolution

Note that all NServiceBus services will be injected into the passed in dependency injection instance. As such there is no need to register these instances at configuration time. 

Note: `IEndpointInstance` needs to be registered to be properly resolved.


### Cleanup

NOTE: In Version 6 `IEndpointInstance` is not `IDisposable`.
