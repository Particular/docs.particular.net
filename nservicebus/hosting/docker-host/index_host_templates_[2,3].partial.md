### Host.cs

The endpoint’s configuration should be added to the `Start()` method:

snippet: DockerStartEndpoint

A corresponding `Stop()` method is available for implementing any operations required to gracefully shut down the endpoint:

snippet: DockerStopEndpoint

Additional methods are provided to handle endpoint failures and exceptions. These can be customized as needed to fit the specific requirements of the endpoint:

snippet: DockerErrorHandling