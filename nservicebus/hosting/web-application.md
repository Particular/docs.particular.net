---
title: Web application hosting
summary: Hosting NServiceBus in a web site or web service
reviewed: 2022-09-12
isLearningPath: true
related:
 - samples/web
 - nservicebus/lifecycle
 - samples/startup-shutdown-sequence
 - samples/endpoint-configuration
 - nservicebus/messaging/callbacks
 - nservicebus/hosting/publishing-from-web-applications
---

NServiceBus can be integrated into any web technology that supports .NET, such as:

* [ASP.NET Core](https://www.asp.net)
* [Windows Communication Foundation (WCF)](https://docs.microsoft.com/en-us/dotnet/framework/wcf/whats-wcf)
* [NancyFX](http://nancyfx.org/)

As most web technologies operate in a scale-out manner, NServiceBus can be hosted in a "Send-only" configuration. In this mode, the web application technology acts as a "forwarder" of messages rather than the "processor". The handling code (MVC controller, NancyFX module, etc) of a given web request simply leverages the NServiceBus send APIs and the messages are processed by a backend service.

## Dependency injection integration

### Using the Generic Host

NServiceBus can be easily integrated into any web host that supports the [Microsoft Generic Host](https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host) using the [NServiceBus Generic Host integration](/nservicebus/hosting/extensions-hosting.md). The Generic Host integration automatically registered `IMessageSession` with the dependency injection container.

### Using self-hosting

Web request handlers (MVC Controllers, WCF Handlers, NancyFx Modules, etc.) require access to the endpoint messaging session in order to [send messages](/nservicebus/messaging/send-a-message.md) as a result of incoming HTTP requests. Many of the supported web application hosts resolve these web request handlers using dependency injection. NServiceBus already creates and manages its own dependency injection when using self-hosting.

The recommended approach to handle this scenario is to have two dependency injection instances: one for the web host and its web request handlers, and another for the NServiceBus endpoint and its message handlers. There are some things to consider when adopting this approach.

* Any service which is registered with the NServiceBus dependency injection can be injected into NServiceBus message handlers but not the web request handlers.
* Any service which is registered with the web host dependency injection mechanism can be injected into web request handlers but not the NServiceBus message handlers.
* Any service which is registered with both dependency injection instances can be resolved in both web request handlers and NServiceBus message bus handlers _but_:
  * these will be different object instances with different lifetimes.
  * even if the services are registered with a singleton lifetime, there will still be one created for each dependency injection instance unless the instance is explicitly managed by the caller.
  * if a service must be shared and a single instance, it must be created externally during the web application host startup, and that specific instance must be registered in both dependency injection instances.

## Endpoint Lifecycle

In a web-hosted scenario, [recycling an IIS process](https://docs.microsoft.com/en-us/previous-versions/iis/6.0-sdk/ms525803(v=vs.90)) causes the hosted NServiceBus endpoint to shutdown and restart.
