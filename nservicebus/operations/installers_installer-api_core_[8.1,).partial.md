## Running installers

When requiring special privileges to setup the endpoint, the installer API can be used to run all necessary installation steps:

snippet: installer-setup

When the setup requires access to services that are provided via an externally managed dependency injection container, provide the configured `IServiceProvider` to the installer API:

snippet: installer-setup-externally-managed-container

Note: The installer APIs always run installers, regardless whether the endpoint has configured `EnableInstallers`.

The installer APIs are intended to be used when the endpoint host process be run with different privileges after installation. The `EndpointConfiguration` can't be reused in the same process to call `Endpoint.Start`. To setup the endpoint as part of the endpoint host process, use see the [Running installers during endpoint startup](#running-installers-during-endpoint-startup).

