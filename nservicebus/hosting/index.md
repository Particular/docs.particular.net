---
title: Hosting
summary: Outlines the various approaches to hosting NServiceBus
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

Related:

 * [Self-Hosting Sample](/samples/hosting/self-hosting/) for more details.

When self-hosting, the user is responsible for creating and starting the endpoint instance

snippet:Hosting-Startup

The user is also responsible for properly shutting down the endpoint when it is no longer needed (usually when the application terminates).

snippet:Hosting-Shutdown

NOTE: In Version 6, the endpoint instance is not disposable due to the asynchronous nature of the pipeline. Call `Stop` in an async manner (see example above).


### Windows Service Hosting

A [Windows Service](https://msdn.microsoft.com/en-us/library/d56de412.aspx) is the most common way NServiceBus is hosted.

**Stopping Windows Service**

NServiceBus gracefully shuts down the Windows Service by default. During a reboot of the machine, Windows will terminate the process if it takes longer than 30 seconds. To prevent endpoints from hanging indefinitely, this feature was also added to the NServiceBus Host itself in version 6.0.1 and now terminates the process after waiting for 30 seconds. This setting can be changed in the registry. More information [can be found here](https://msdn.microsoft.com/en-us/library/windows/desktop/ms685149(v=vs.85).aspx).

Related:

 * [Hosting NServiceBus in Windows Service](windows-service.md)
 * [Windows-Service Hosting Sample](/samples/hosting/windows-service/)
 * [Endpoint Configuration Choices](/samples/endpoint-configuration/)


### Send-only hosting

A "Send-only endpoint" is used when the only purpose is sending messages and no message processing is required in that endpoint. Common use cases include websites, console application and windows application. This is the code for starting an endpoint in send only mode.

snippet:Hosting-SendOnly

The only configuration when running in this mode is the destination when [Sending a message](/nservicebus/messaging/send-a-message.md).


### Web Hosting

NServiceBus can be hosted in any web technology that support .NET. This includes

 * ASP.net
 * ASP.MVC
 * WCF
 * Web API
 * NancyFX

And many others.

As most web technologies operate in a scale out manner NServiceBus is **usually** hosted in a "Send-only" manner. In this mode they act as a "forwarder" of messages rather than the "processor". So the handling code (MVC controller, NancyFX module etc) of a given web request simply leverages the `Bus` send APIs and no processing is done in the web process. The actually message handling is done in a [Windows Service Endpoint](windows-service.md).

NOTE: There are some [Caveats when publishing from a Web Application](publishing-from-web-applications.md).

NOTE: In a web hosted scenario a [IIS Recycle](https://msdn.microsoft.com/en-us/library/ms525803.aspx) is considered a shutdown and restart of the bus.

Related:

 * [Web Samples](/samples/web/)
 * [Handling Responses on the Client Side](/nservicebus/messaging/handling-responses-on-the-client-side.md)


### Multi-Hosting

"Multi-hosting" refers to hosing multiple NServiceBus endpoints in a single .NET process. In Version 4 and earlier this could be achieved through multiple AppDomains. In Version 5 and higher multiple endpoints can share the same AppDomain or use the multiple AppDomains approach.

Related:

 * [Multi-Hosting Sample](/samples/hosting/multi-hosting/).


### Accessing the bus

Most usages of the bus will occur where the NServiceBus APIs are used. For example [Handlers](/nservicebus/handlers/) and [Sagas](/nservicebus/sagas/). However there are other scenarios that may require an alternative approach where the user needs to directly access the bus from outside of the framework.


#### Using a Container

NServiceBus support dependency injection via use [Containers](/nservicebus/containers/). At startup, the instance of a bus session will be injected into the configured container and can be access via that container.

Related:

 * [Injecting the Bus into ASP.NET MVC Controller](/samples/web/asp-mvc-injecting-bus/)


NOTE: Since Version 6, `IEndpointInstance`/`IBusSession` (the equivalent of `IBus` in earlier versions) is no longer automatically injected into the container. In order to send messages you need to explicitly create a bus context. Here's a sample code showing how to automate this task using the Autofac container

snippet:Hosting-Inject


#### Static variable

For many scenarios a container is not required. In these cases a simple public static variable on the startup class will suffice. This variable can then be access globally in your application. For example:

 * In windows service or console the variable would be placed on the `Program.cs`
 * In a Website the variable would be placed on the `Global.cs`.

Alternatively the static variable could be placed on a (more appropriately named) helper class.

snippet:Hosting-Static


## "Custom Host" Solutions

A "Custom Host" refers to a process or library that wraps the NServiceBus library to take partial control of configuration, startup and shutdown. This Host then exposes extension points for common activities and uses conventions and/or sensible defaults for many other configuration options.


### NServiceBus Host

The [NServiceBus Host](/nservicebus/hosting/nservicebus-host/) takes a more opinionated approach to hosting. It allows the execution as both a windows service and a console application (for development). It also adds the concepts of [Profiles](/nservicebus/hosting/nservicebus-host/profiles.md) and [Custom installation](/nservicebus/hosting/nservicebus-host/#installation).

Related:

 * [NServiceBus Host Sample](/samples/hosting/nservicebus-host/)


### Hosting in Azure

There are a variety of ways to host in Azure. Depending on your requirements Self Hosting may be an option or, alternatively, a custom Azure host may be required. See [Hosting in Azure Cloud Services](/nservicebus/azure/hosting-in-azure-cloud-services.md) and [Hosting in Azure](/nservicebus/azure/hosting.md) for more information.

Related:

 * [Shared Hosting in Azure Cloud Services Sample](/samples/azure/shared-host/)
