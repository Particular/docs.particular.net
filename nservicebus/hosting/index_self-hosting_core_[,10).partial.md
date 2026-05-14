## Self-hosting

"Self-hosting" is a general term used when the application code takes full control over all facets of hosting NServiceBus. This includes the following actions:

 * Configuration
 * [Logging](/nservicebus/logging)
 * [Dependency injection](/nservicebus/dependency-injection/)
 * [Endpoint Lifecycle](/nservicebus/lifecycle/)
 * [Critical Error handling](critical-errors.md)

> [!NOTE]
> It is recommended that the default critical error callback be overridden when self-hosting NServiceBus. Refer to the [Critical Errors](/nservicebus/hosting/critical-errors.md) article for more information.

When self-hosting, the user is responsible for creating and starting the endpoint instance:

snippet: Hosting-Startup

The user is also responsible for properly shutting down the endpoint when it is no longer needed (usually when the application terminates).

snippet: Hosting-Shutdown

> [!NOTE]
> The endpoint instance is not disposable due to the asynchronous nature of the pipeline. Call `Stop` in an async manner (see example above).