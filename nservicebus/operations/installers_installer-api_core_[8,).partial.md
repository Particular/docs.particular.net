## Running installers

When requiring special privileges to setup the endpoint, the installer API can be used to run all necessary installation steps:

snippet: installer-setup

When the setup requires access to services that are provided via an externally managed dependency injection container, provide the configured `IServiceProvider` to the installer API:

snippet: installer-setup-externally-managed-container

> [!NOTE]
> The installer APIs always run installers, regardless of whether the endpoint has configured `EnableInstallers`.

The installer APIs are intended to be used when resources required by the endpoint must be set up with different privileges than the endpoint itself requires. `EndpointConfiguration` can't be reused in the same process to call `Endpoint.Start`. To setup the endpoint as part of the endpoint host process, see the [Running installers during endpoint startup](#running-installers-during-endpoint-startup) article.

