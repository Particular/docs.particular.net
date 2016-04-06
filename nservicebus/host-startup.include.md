## Startup Behavior

For Versions 7 and above, a new interface called `IWantToRunWhenEndpointStartsAndStops` has been added. This interface allows code to be executed at startup and shutdown of the Host. In Versions 6 and use the [Endpoint Instance Start and Stop](/nservicebus/lifecycle/endpointstartandstop.md) functionality`.

At startup, the Host invokes all the classes that implement the `IWantToRunWhenEndpointStartsAndStops`interface.

WARNING: Implementations of `IWantToRunWhenEndpointStartsAndStops` are not started and stopped on a dedicated thread. They are executed on the thread starting and disposing the endpoint. It is the responsibility of the implementing class to execute its operations in parallel if needed (i.e. for CPU bound work). Failure to do so will prevent the endpoint from being started and/or disposed. 

 * Instances of `IWantToRunWhenEndpointStartsAndStops` are located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md) and automatically registered into the [configured container](/nservicebus/containers/) during endpoint creation. These are registered as `Instance Per Call`.
 * They are started before the transport and any satellites have started. Therefore the endpoint will not receive any messages until this process has completed.
 * These instances are created by the [Container](/nservicebus/containers/) which means they:
  * Will have dependencies injected.
  * Do not require a default constructor.
 * These instances will be started asynchronously in the same method which started the bus.
 * Once created `Start` is called on each instance asynchronously without awaiting its completion. Each call to `Start` is called in the `OnStart` method of a [FeatureStartupTask](/nservicebus/pipeline/features.md#feature-startup-tasks). 
 * Instances of `IWantToRunWhenEndpointStartsAndStops` which successfully start are kept internally to be stopped when the endpoint is stopped.
 * When the endpoint is shut down, the `Stop` method for each instance of `IWantToRunWhenEndpointStartsAndStops` is called asynchronously. Each call to `Stop` is called in the `OnStop` method of a [FeatureStartupTask](/nservicebus/pipeline/features.md#feature-startup-tasks).
 * The instances will be stopped only after the transport and any satellites have stopped. While all instances of `IWantToRunWhenEndpointStartsAndStops` are being stopped, the endpoint will not handle any messages received.
 * The instances will be stopped asynchronously within the same method which disposed the endpoint.

NOTE: The endpoint will not start processing messages until all instances of `IWantToRunWhenEndpointStartsAndStops.Start` are completed.

DANGER: The `Start` and `Stop` methods will block start up and shut down of the endpoint. For any long running methods, use `Task.Run` so as not to block execution.

include: non-null-task


### Exceptions

Exceptions thrown in the constructors of instances of `IWantToRunWhenEndpointStartsAndStops` are unhandled by NServiceBus. These will prevent the endpoint from starting up.

Exceptions raised from the `Start` method will prevent the endpoint from starting. As they are called asynchronously and awaited with `Task.WhenAll`, an exception may prevent other `Start` methods from being called. 

Exceptions raised from the `Stop` method will not prevent the endpoint from shutting down. The exceptions will be logged at the Fatal level.