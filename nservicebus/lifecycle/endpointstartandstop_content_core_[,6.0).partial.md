Instances are:

 * Located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md) and automatically registered into the [configured dependency injection](/nservicebus/dependency-injection/) during endpoint creation. These are registered as `Instance Per Call`.
 * Created and started as the last step when the endpoint is started.
 * Started after the Transport and any Satellites have started. While all instances of `IWantToRunWhenBusStartsAndStops` are being started, no message will be processed.
 * Created on the same thread that is starting the endpoint.
 * Created by [dependency injection](/nservicebus/dependency-injection/) which means they:
  * Will have dependencies injected.
  * Do not require a default constructor.

Once created `Start` is called on each instance in parallel. Each `Start` call is made on a different background thread. Instances of `IWantToRunWhenBusStartsAndStops` are kept internally to be stopped when the endpoint is disposed.

NOTE: The call to `IStartableBus.Start` may return before all instances of `IWantToRunWhenBusStartsAndStops.Start` are completed.

When the endpoint is disposed, all instances of `IWantToRunWhenBusStartsAndStops` are stopped by calling their `Stop` method. Each call to `Stop` happens on parallel background threads but the call to `Dispose()` will block until they have all completed.

NOTE: `Stop` will wait for any outstanding instances of `Start` to complete. If an instance of `IWantToRunWhenBusStartsAndStops` needs to be long running then it must start it's own background thread. Failure to do so will prevent the endpoint from being disposed.


### Exceptions

Exceptions thrown in the constructors of instances of the start and stop interfaces are unhandled by NServiceBus. These will bubble up to the code that starts the the endpoint.

Exceptions raised from the `Start` method will cause a [Critical Error](/nservicebus/hosting/critical-errors.md). As they are run on separate threads, an exception on one `Start` call will not interfere with any others.

Exceptions raised from the `Stop` method will not halt shutdown and will be logged at the Fatal level.


## Code

snippet: lifecycle-EndpointStartAndStopCore