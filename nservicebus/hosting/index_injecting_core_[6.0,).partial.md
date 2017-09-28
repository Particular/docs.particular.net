NOTE: In Versions 6 and above, `IEndpointInstance`/`IMessageSession` (the equivalent of `IBus` in earlier versions) is no longer automatically injected via [dependency injection](/nservicebus/dependency-injection/). In order to send messages explicitly create a bus context. Here's a sample code showing how to automate this task using Autofac.

snippet: Hosting-Inject
