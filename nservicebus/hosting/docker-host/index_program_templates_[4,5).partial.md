### Program.cs

The endpoint’s configuration should be added to the `CreateHostBuilder` method:

snippet: DockerCreateHostBuilder

Additional methods are available to handle endpoint failures and exceptions. These can be customized as needed to suit the specific behavior of the endpoint:

snippet: DockerErrorHandling