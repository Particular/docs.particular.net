---
title: Hosting
summary: Outlines the various approaches to endpoint hosting
component: Core
reviewed: 2017-10-12
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
 * [Dependency injection](/nservicebus/dependency-injection/)
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


#### Using dependency injection

NServiceBus supports [dependency injection](/nservicebus/dependency-injection/). At startup, the instance of a bus session will be injected into the configured dependency injection and can be access via that instance.

Related:

 * [Sending from an ASP.NET MVC Controller](/samples/web/send-from-mvc-controller/)


partial: injecting


#### Static variable

For many scenarios dependency injection is not required. In these cases a simple public static variable on the startup class will suffice. This variable can then be access globally in the application. For example:

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

Beyond virtual machines Azure provides a number of ways to host an NServiceBus endpoint.

#### Azure Cloud Services

[Azure Cloud services](https://azure.microsoft.com/en-us/services/cloud-services/) is a Platform-as-a-Service (PaaS) option for hosting one or more NServiceBus endpoints. See the following articles and samples for more information:

 * [Hosting in Azure Cloud Services](/nservicebus/hosting/cloud-services-host/)
 * [Shared Hosting in Azure Cloud Services Sample](/samples/azure/shared-host/)

#### Azure App Services
[Azure App Services](https://azure.microsoft.com/en-us/services/app-service/) is another PaaS option which provides the capability to web host NServiceBus endpoints. By adding one or more continuously running Web Jobs to your App Service NServiceBus endpoints can also be hosted as a background process. See the following articles and samples for more information:

* [Hosting in a continuously running Web Job](/nservicebus/hosting/azure-appservice-webjob-hosting)
* [Sending from an ASP.NET MVC Controller](/samples/web/send-from-mvc-controller/)

#### Service Fabric on Azure
[Service Fabric on Azure](https://azure.microsoft.com/en-us/services/service-fabric/) provides a way to leverage the same highly available and scalable infrastructure that powers many of Azure's own services. See the following articles and samples for more information:

* [Service Fabric Hosting](/nservicebus/hosting/service-fabric-hosting)
* [Service Fabric Partition Aware Routing](/samples/azure/azure-service-fabric-routing/)

#### Azure Container Services
[Azure Container Services](https://azure.microsoft.com/en-us/services/container-service/) provides a way to deploy and run Docker containers in Azure. By containerizing your NServiceBus self-hosted endpoint you can take advantage of Docker capabilities in the Azure cloud.

 * [Hosting your endpoints in Docker Linux containers](/samples/hosting/docker/)


Related:

 
  
 
## ILMerging NServiceBus assemblies

Since NServiceBus makes assumptions on aspects like assembly names, ILMerging any of the NServiceBus* assemblies is not supported.
