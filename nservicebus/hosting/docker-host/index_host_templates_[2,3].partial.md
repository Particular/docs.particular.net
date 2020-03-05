### Host.cs

The endpoint's configuration must be added to the `Start()` method. 

snippet: DockerStartEndpoint

There is also a `Stop()` method that can hold any operations required to gracefully shutdown the endpoint.

snippet: DockerStopEndpoint

There are also methods that handle endpoint failures and exceptions, which can be modified to fit the needs of the endpoint.

snippet: DockerErrorHandling