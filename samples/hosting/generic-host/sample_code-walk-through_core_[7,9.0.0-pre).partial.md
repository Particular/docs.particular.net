## Code walk-through

snippet: generic-host-service-lifetime

The snippet above shows how the host builder runs by default as a Windows Service. If the sample is started with the debugger attached, it uses the console's lifetime instead. To always use the console lifetime use the following code instead:

snippet: generic-host-console-lifetime

Next, the builder configures NServiceBus using the [`NServiceBus.Extensions.Hosting`](/nservicebus/hosting/extensions-hosting.md) package, including the [critical error](/nservicebus/hosting/critical-errors.md) action that will shut down the application or service in case of a critical error.

snippet: generic-host-nservicebus

The critical error action:

snippet: generic-host-critical-error

To simulate work, a BackgroundService called `Worker` is registered as a hosted service:

snippet: generic-host-worker-registration

The `IMessageSession` is injected into the `Worker` constructor, and the `Worker` sends messages when it is executed.

snippet: generic-host-worker
