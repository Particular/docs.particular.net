## Container configuration

include: internallymanagedcontainer

### Injecting the endpoint instance

`IEndpointInstance` is not registered automatically and must be registered explicitly to be injected.

NOTE: In NServiceBus version 6 and above `IEndpointInstance` is not `IDisposable`.
