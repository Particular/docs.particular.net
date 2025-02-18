---
title: Hosting
summary: Describes the various approaches to endpoint hosting
component: Core
reviewed: 2025-01-13
redirects:
- nservicebus/hosting/self-hosting
- nservicebus/hosting/self-hosting-v4.x
- nservicebus/hosting/send-only-endpoints
---

NServiceBus is a library at its core so that it can be hosted in any .NET process.

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

> [!NOTE]
> It is recommended that the default critical error callback be overridden when self-hosting NServiceBus. Refer to the [Critical Errors](/nservicebus/hosting/critical-errors.md) article for more information.

When self-hosting, the user is responsible for creating and starting the endpoint instance:

snippet: Hosting-Startup

The user is also responsible for properly shutting down the endpoint when it is no longer needed (usually when the application terminates).

snippet: Hosting-Shutdown

> [!NOTE]
> The endpoint instance is not disposable due to the asynchronous nature of the pipeline. Call `Stop` in an async manner (see example above).

### Windows Service hosting

A [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) is a common way to host NServiceBus in Windows.

Related:
 * [Generic host as Windows Service](/samples/hosting/generic-host)
 * [Generate a Windows Service project using `dotnet new`](/nservicebus/dotnet-templates/)
 * [Windows Service installation](windows-service.md)

### Docker container hosting

An endpoint can be hosted inside a [Docker](https://www.docker.com/) container.

Related:
 * [Docker container host](/nservicebus/hosting/docker-host/)
 * [Generate a Docker-hosted endpoint project with `dotnet new`](/nservicebus/dotnet-templates/)
 * [Hosting endpoints in Docker Linux containers](/samples/hosting/docker/)
 * [Generic host](/samples/hosting/generic-host)

### Send-only hosting

A "Send-only endpoint" is used when the only purpose is sending messages, and no message processing is required. Common use cases include websites, console applications, and Windows applications. This is the code for starting an endpoint in send-only mode.

snippet: Hosting-SendOnly

The only configuration required when running in this mode is the destination when [sending a message](/nservicebus/messaging/send-a-message.md).

### Web hosting

NServiceBus can be hosted using any web technology that supports .NET. See [Web Application Hosting](web-application.md) for more information.

### WebJob hosting

NServiceBus can be hosted in a WebJob. See [Self-Hosting in Azure WebJobs](/samples/azure/webjob-host)

### Service Fabric hosting

[Service Fabric](https://docs.microsoft.com/en-us/azure/service-fabric/) can host NServiceBus endpoints in several ways. See [Service Fabric Hosting](/nservicebus/hosting/service-fabric-hosting).

### Serverless hosting

NServiceBus can be hosted in several serverless environments such as [Azure Functions](/nservicebus/hosting/azure-functions-service-bus/) and [AWS Lambda](/nservicebus/hosting/aws-lambda-simple-queue-service/).

### Accessing the bus

Most usages of the bus will occur where the NServiceBus APIs are used, for example, [handlers](/nservicebus/handlers/) and [sagas](/nservicebus/sagas/). However, other scenarios may require an alternate approach where the user needs to access the bus from outside of the framework directly.

#### Static variable

For many scenarios, dependency injection is not required. In these cases, a simple public static variable on the startup class will suffice. This variable can then be accessed globally within the application. For example:

 * In a Windows service or console, the variable could be placed in `Program.cs`
 * In a website, the variable could be placed in `Global.asax.cs`.

The static variable could also be placed in a helper class.

snippet: Hosting-Static

## Custom hosting

A "Custom host" is a process or library that wraps the NServiceBus library to control configuration, startup, and shutdown partially. This host exposes extension points for common activities and uses conventions and/or sensible defaults for many other configuration options.

## ILMerging NServiceBus assemblies

Since NServiceBus makes assumptions on aspects like assembly names, ILMerging any of the NServiceBus* assemblies is not supported.

## Hosting environment requirements

NServiceBus endpoints have certain requirements for the hosting environment:

* The endpoint process needs write access to write log files. See the [logging documentation](/nservicebus/logging) for more details about the default log file location and how to configure logging.
* The endpoint process needs write access to write the startup diagnostics file. See the [startup diagnostics documentation](/nservicebus/hosting/startup-diagnostics.md) for more details about the diagnostic file.
