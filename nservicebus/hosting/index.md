---
title: Hosting
summary: Describes the various approaches to endpoint hosting
component: Core
reviewed: 2018-04-06
tags:
- Hosting
redirects:
- nservicebus/hosting/self-hosting
- nservicebus/hosting/self-hosting-v4.x
- nservicebus/hosting/send-only-endpoints
---

At its core NServiceBus is a library; as such it can be hosted in any .NET process.

There are several approaches to hosting.

## Self-hosting

"Self-hosting" is a general term used when the application code takes full control over all facets of hosting NServiceBus. This includes the following actions:

 * Configuration
 * [Logging](/nservicebus/logging)
 * [Dependency injection](/nservicebus/dependency-injection/)
 * [Startup and Shutdown](/samples/startup-shutdown-sequence/)
 * [Endpoint Lifecycle](/nservicebus/lifecycle/)
 * [Critical Error handling](critical-errors.md)

Note: Override the default critical error callback when self-hosting NServiceBus. By default NServiceBus will stop the endpoint instance without exiting the process. Refer to the [Critical Errors](/nservicebus/hosting/critical-errors.md) article for more information.

Related:

 * [Self-Hosting Sample](/samples/hosting/self-hosting/) for more details.

When self-hosting, the user is responsible for creating and starting the endpoint instance:

snippet: Hosting-Startup

The user is also responsible for properly shutting down the endpoint when it is no longer needed (usually when the application terminates).

snippet: Hosting-Shutdown

partial: dispose


### Windows Service hosting

A [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) is the most common way to host NServiceBus in Windows.

Related:

 * [Hosting NServiceBus in Windows Service](windows-service.md)
 * [Windows-Service Hosting Sample](/samples/hosting/windows-service/)
 * [Endpoint Configuration Choices](/samples/endpoint-configuration/)


### Send-only hosting

A "Send-only endpoint" is used when the only purpose of the endpoint is to send messages and no message processing is required in that endpoint. Common use cases include websites, console applications, and windows applications. This is the code for starting an endpoint in send-only mode.

snippet: Hosting-SendOnly

The only configuration required when running in this mode is the destination when [sending a message](/nservicebus/messaging/send-a-message.md).


### Web hosting

NServiceBus can be hosted in any web technology that supports .NET. See [Web Application Hosting](web-application.md).


### Service Fabric hosting

[Service Fabric](https://docs.microsoft.com/en-us/azure/service-fabric/) can be used to host NServiceBus endpoints in several ways. See [Service Fabric Hosting](/nservicebus/hosting/service-fabric-hosting).


### Multi-hosting

"Multi-hosting" refers to hosting multiple NServiceBus endpoints in a single .NET process.

partial: multi

Related:

 * [Multi-Hosting Sample](/samples/hosting/multi-hosting/).


### Accessing the bus

Most usages of the bus will occur where the NServiceBus APIs are used, for example [handlers](/nservicebus/handlers/) and [sagas](/nservicebus/sagas/). However, there are other scenarios that may require an alternate approach where the user needs to directly access the bus from outside of the framework.


#### Using dependency injection

NServiceBus supports [dependency injection](/nservicebus/dependency-injection/). At startup, the instance of a bus session will be injected into the configured dependency injection and can be access via that instance.

Related:

 * [Sending from an ASP.NET MVC Controller](/samples/web/send-from-mvc-controller/)


partial: injecting


#### Static variable

For many scenarios, dependency injection is not required. In these cases, a simple public static variable on the startup class will suffice. This variable can then be access globally in the application. For example:

 * In a Windows service or console the variable could be placed in `Program.cs`
 * In a website the variable could be placed in `Global.asax.cs`.

The static variable could also be placed in a helper class.

snippet: Hosting-Static


## Custom hosting

A "Custom host" is a process or library that wraps the NServiceBus library to take partial control of configuration, startup and shutdown. This host exposes extension points for common activities and uses conventions and/or sensible defaults for many other configuration options.


### NServiceBus host

The [NServiceBus host](/nservicebus/hosting/nservicebus-host/) takes a more opinionated approach to hosting. It allows the execution as both a Windows service and a console application (for development). It also adds the concepts of [profiles](/nservicebus/hosting/nservicebus-host/profiles.md) and [custom installation](/nservicebus/hosting/nservicebus-host/installation.md).

Related:

 * [NServiceBus Host Sample](/samples/hosting/nservicebus-host/)


### Hosting in Azure

There are multiple ways to host in Azure. Depending on the requirements, self-hosting may be an option or a custom Azure host may be required. See [Hosting in Azure Cloud Services](/nservicebus/hosting/cloud-services-host/) for more information.

Related:

 * [Shared Hosting in Azure Cloud Services Sample](/samples/azure/shared-host/)
 

partial: diagnostics

## ILMerging NServiceBus assemblies

Since NServiceBus makes assumptions on aspects like assembly names, ILMerging any of the NServiceBus* assemblies is not supported.
