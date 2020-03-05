### Program.cs

The endpoint's configuration must be added to the `CreateHostBuilder` method. 

snippet: DockerCreateHostBuilder

There are also methods that handle endpoint failures and exceptions, which can be modified to fit the needs of the endpoint.

snippet: DockerErrorHandling