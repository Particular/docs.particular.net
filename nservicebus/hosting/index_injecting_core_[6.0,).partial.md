NOTE: In Versions 6 and above, `IEndpointInstance`/`IMessageSession` (the equivalent of `IBus` in earlier versions) is no longer automatically injected into the container. In order to send messages explicitly create a bus context. Here's a sample code showing how to automate this task using the Autofac container

snippet:Hosting-Inject
