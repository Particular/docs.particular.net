---
title: Hosting
summary: Describes the various approaches to endpoint hosting
component: Core
reviewed: 2026-05-04
redirects:
- nservicebus/hosting/self-hosting
- nservicebus/hosting/self-hosting-v4.x
- nservicebus/hosting/send-only-endpoints
- nservicebus/hosting/service-fabric-hosting
---

NServiceBus is a library at its core so that it can be hosted in any .NET process.

There are several approaches to hosting.

partial: generic-host

partial: self-hosting

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

## Send-only hosting

A "Send-only endpoint" is used when the only purpose is sending messages, and no message processing is required. Common use cases include websites, console applications, and Windows applications. This is the code for starting an endpoint in send-only mode.

snippet: Hosting-SendOnly

The only configuration required when running in this mode is the destination when [sending a message](/nservicebus/messaging/send-a-message.md).

## Hosting technologies

### Web hosting

NServiceBus can be hosted using any web technology that supports .NET. See [Web Application Hosting](web-application.md) for more information.

### WebJob hosting

NServiceBus can be hosted in a WebJob. See [Self-Hosting in Azure WebJobs](/samples/azure/webjob-host)

### Serverless hosting

NServiceBus can be hosted in several serverless environments such as [Azure Functions](/nservicebus/hosting/azure-functions-service-bus/) and [AWS Lambda](/nservicebus/hosting/aws-lambda-simple-queue-service/).

## Accessing the bus

Most usages of the bus will occur where the NServiceBus APIs are used, for example, [handlers](/nservicebus/handlers/) and [sagas](/nservicebus/sagas/). However, other scenarios may require an alternate approach where the user needs to access the bus from outside of the framework directly.

### Static variable

For many scenarios, dependency injection is not required. In these cases, a simple public static variable on the startup class will suffice. This variable can then be accessed globally within the application. For example:

 * In a Windows service or console, the variable could be placed in `Program.cs`
 * In a website, the variable could be placed in `Global.asax.cs`.

The static variable could also be placed in a helper class.

snippet: Hosting-Static

## Hosting environment requirements

NServiceBus endpoints have certain requirements for the hosting environment:

* The endpoint process needs write access to write log files. See the [logging documentation](/nservicebus/logging) for more details about the default log file location and how to configure logging.
* The endpoint process needs write access to write the startup diagnostics file. See the [startup diagnostics documentation](/nservicebus/hosting/startup-diagnostics.md) for more details about the diagnostic file.
* Since NServiceBus makes assumptions on aspects like assembly names, ILMerging any of the NServiceBus* assemblies is not supported.
