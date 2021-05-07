---
title: Hosting
summary: Describes the various approaches to endpoint hosting
component: Core
reviewed: 2020-01-03
redirects:
- nservicebus/hosting/self-hosting
- nservicebus/hosting/self-hosting-v4.x
- nservicebus/hosting/send-only-endpoints
---

At its core NServiceBus is a library; as such it can be hosted in any .NET process.

There are several approaches to hosting.

## Microsoft Generic Host

The [Microsoft Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host) is the most common way to host NServiceBus on .NET Core. NServiceBus can be integrated with the generic host using the [NServiceBus.Extensions.Hosting package](/nservicebus/hosting/extensions-hosting.md).


## Self-hosting

"Self-hosting" is a general term used when the application code takes full control over all facets of hosting NServiceBus. This includes the following actions:

 * Configuration
 * [Logging](/nservicebus/logging)
 * [Dependency injection](/nservicebus/dependency-injection/)
 * [Startup and Shutdown](/samples/startup-shutdown-sequence/)
 * [Endpoint Lifecycle](/nservicebus/lifecycle/)
 * [Critical Error handling](critical-errors.md)

Note: Override the default critical error callback when self-hosting NServiceBus. Refer to the [Critical Errors](/nservicebus/hosting/critical-errors.md) article for more information.

When self-hosting, the user is responsible for creating and starting the endpoint instance:

snippet: Hosting-Startup

The user is also responsible for properly shutting down the endpoint when it is no longer needed (usually when the application terminates).

snippet: Hosting-Shutdown

partial: dispose


### Windows Service hosting

A [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) is the most common way to host NServiceBus in Windows.

Related:
 * [Generic host as Windows Service](/samples/hosting/generic-host)
 * [NServiceBus Windows Service template](/nservicebus/dotnet-templates.md#nservicebus-windows-service)
 * [Windows Service installation](windows-service.md)
 * [Endpoint configuration choices](/samples/endpoint-configuration/)



### Docker container hosting

An endpoint can be hosted inside a [Docker](https://www.docker.com/) container.

Related:
 * [Docker container host](/nservicebus/hosting/docker-host/)
 * [NServiceBus Docker container template](/nservicebus/dotnet-templates.md#nservicebus-docker-container)
 * [Hosting endpoints in Docker Linux containers](/samples/hosting/docker/)
 * [Generic host](/samples/hosting/generic-host)

### Send-only hosting

A "Send-only endpoint" is used when the only purpose of the endpoint is to send messages and no message processing is required in that endpoint. Common use cases include websites, console applications, and windows applications. This is the code for starting an endpoint in send-only mode.

snippet: Hosting-SendOnly

The only configuration required when running in this mode is the destination when [sending a message](/nservicebus/messaging/send-a-message.md).


### Web hosting

NServiceBus can be hosted in any web technology that supports .NET. See [Web Application Hosting](web-application.md).

### WebJob hosting

NServiceBus can be hosted in a WebJob. See [Self-Hosting in Azure WebJobs](/samples/azure/webjob-host)

### Service Fabric hosting

[Service Fabric](https://docs.microsoft.com/en-us/azure/service-fabric/) can be used to host NServiceBus endpoints in several ways. See [Service Fabric Hosting](/nservicebus/hosting/service-fabric-hosting).

### Serverless hosting

NServiceBus can be hosted in several serverless environments such as [Azure Functions](/nservicebus/hosting/azure-functions/service-bus.md) and [AWS Lambda](/previews/aws-lambda-simple-queue-service.md).

### Multi-hosting

"Multi-hosting" refers to hosting multiple NServiceBus endpoints in a single .NET process.

It is safe for multiple endpoints to share the same AppDomain or use multiple AppDomains.

Related:

 * [Multi-Hosting in the generic host](/samples/hosting/generic-multi-hosting/).

partial: reusing-endpointconfig

### Accessing the bus

Most usages of the bus will occur where the NServiceBus APIs are used, for example [handlers](/nservicebus/handlers/) and [sagas](/nservicebus/sagas/). However, there are other scenarios that may require an alternate approach where the user needs to directly access the bus from outside of the framework.


#### Static variable

For many scenarios, dependency injection is not required. In these cases, a simple public static variable on the startup class will suffice. This variable can then be access globally in the application. For example:

 * In a Windows service or console the variable could be placed in `Program.cs`
 * In a website the variable could be placed in `Global.asax.cs`.

The static variable could also be placed in a helper class.

snippet: Hosting-Static


## Custom hosting

A "Custom host" is a process or library that wraps the NServiceBus library to take partial control of configuration, startup and shutdown. This host exposes extension points for common activities and uses conventions and/or sensible defaults for many other configuration options.


## ILMerging NServiceBus assemblies

Since NServiceBus makes assumptions on aspects like assembly names, ILMerging any of the NServiceBus* assemblies is not supported.


## Hosting environment requirements

NServiceBus endpoints have certain requirements on the hosting environment:

* The endpoint process needs write access to write log files. See the [logging documentation](/nservicebus/logging) for more details about the default log file location and how to configure logging.
* The endpoint process needs write access to write the startup diagnostics file. See the [startup diagnostics documentation](/nservicebus/hosting/startup-diagnostics.md) for more details about the diagnostic file.
