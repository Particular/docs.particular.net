---
title: Hosting
summary: Outlines the various approaches to endpoint hosting
component: Core
reviewed: 2016-08-24
tags:
- Hosting
redirects:
- nservicebus/hosting/self-hosting
- nservicebus/hosting/self-hosting-v4.x
- nservicebus/hosting/send-only-endpoints
---

At its core NServiceBus is a library, as such it can be hosted in any .NET process.

There are several approaches to hosting.


## Self-hosting

"Self-hosting" is a general term used to refer to when the application code takes full control over all facets of hosting NServiceBus. This includes the following actions:

 * Configuration
 * [Logging](/nservicebus/logging)
 * [Container](/nservicebus/containers/)
 * [Startup and Shutdown](/samples/startup-shutdown-sequence/)
 * [Endpoint Lifecycle](/nservicebus/lifecycle/)
 * [Critical Error handling](critical-errors.md)

Note: Override the default critical error callback when self-hosting NServiceBus. By default NServiceBus will stop the endpoint instance without exiting the process. Refer to the [Critical Errors](/nservicebus/hosting/critical-errors.md) article for more information.

Related:

 * [Self-Hosting Sample](/samples/hosting/self-hosting/) for more details.

When self-hosting, the user is responsible for creating and starting the endpoint instance

snippet: Hosting-Startup

The user is also responsible for properly shutting down the endpoint when it is no longer needed (usually when the application terminates).

snippet: Hosting-Shutdown

partial: dispose


### Windows Service Hosting

A [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) is the most common way NServiceBus is hosted.

Related:

 * [Hosting NServiceBus in Windows Service](windows-service.md)
 * [Windows-Service Hosting Sample](/samples/hosting/windows-service/)
 * [Endpoint Configuration Choices](/samples/endpoint-configuration/)


### Send-only hosting

A "Send-only endpoint" is used when the only purpose is sending messages and no message processing is required in that endpoint. Common use cases include websites, console application and windows application. This is the code for starting an endpoint in send only mode.

snippet: Hosting-SendOnly

The only configuration when running in this mode is the destination when [Sending a message](/nservicebus/messaging/send-a-message.md).


### Web Hosting

NServiceBus can be hosted in any web technology that support .NET. See [Web Application Hosting](web-application.md).


### Service Fabric Hosting

[Service Fabric](https://docs.microsoft.com/en-us/azure/service-fabric/) can be used to host NServiceBus endpoints in several ways. See [Service Fabric Hosting](/nservicebus/hosting/service-fabric-hosting).


### Multi-Hosting

"Multi-hosting" refers to hosting multiple NServiceBus endpoints in a single .NET process.

partial: multi

Related:

 * [Multi-Hosting Sample](/samples/hosting/multi-hosting/).


### Accessing the bus

Most usages of the bus will occur where the NServiceBus APIs are used. For example [Handlers](/nservicebus/handlers/) and [Sagas](/nservicebus/sagas/). However there are other scenarios that may require an alternative approach where the user needs to directly access the bus from outside of the framework.


#### Using a Container

NServiceBus support dependency injection via use [Containers](/nservicebus/containers/). At startup, the instance of a bus session will be injected into the configured container and can be access via that container.

Related:

 * [Sending from an ASP.NET MVC Controller](/samples/web/send-from-mvc-controller/)


partial: injecting


#### Static variable

For many scenarios a container is not required. In these cases a simple public static variable on the startup class will suffice. This variable can then be access globally in the application. For example:

 * In windows service or console the variable would be placed on the `Program.cs`
 * In a Website the variable would be placed on the `Global.cs`.

Alternatively the static variable could be placed on a (more appropriately named) helper class.

snippet: Hosting-Static


## "Custom Host" Solutions

A "Custom Host" refers to a process or library that wraps the NServiceBus library to take partial control of configuration, startup and shutdown. This Host then exposes extension points for common activities and uses conventions and/or sensible defaults for many other configuration options.


### NServiceBus Host

The [NServiceBus Host](/nservicebus/hosting/nservicebus-host/) takes a more opinionated approach to hosting. It allows the execution as both a windows service and a console application (for development). It also adds the concepts of [Profiles](/nservicebus/hosting/nservicebus-host/profiles.md) and [Custom installation](/nservicebus/hosting/nservicebus-host/installation.md).

Related:

 * [NServiceBus Host Sample](/samples/hosting/nservicebus-host/)


### Hosting in Azure

There are a variety of ways to host in Azure. Depending on the requirements self-hosting may be an option or, alternatively, a custom Azure host may be required. See [Hosting in Azure Cloud Services](/nservicebus/hosting/cloud-services-host/) for more information.

Related:

 * [Shared Hosting in Azure Cloud Services Sample](/samples/azure/shared-host/)
 
 
## ILMerging NServiceBus assemblies

ILMerging any of the NServiceBus* assemblies is not supported.
