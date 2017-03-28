---
title: Web Application Hosting
summary: Hosting NServiceBus in a Web Site or Web Service
reviewed: 2016-05-16
tags:
- Hosting
related:
 - samples/web
 - nservicebus/lifecycle
 - samples/startup-shutdown-sequence
 - samples/endpoint-configuration
 - nservicebus/messaging/callbacks
 - nservicebus/hosting/publishing-from-web-applications
---


NServiceBus can be hosted in any web technology that support .NET. This includes, but not limited to, the following:

 * [ASP.net](https://www.asp.net/get-started/websites)
 * [ASP.MVC](https://www.asp.net/mvc)
 * [Windows Communication Foundation (WCF)](https://msdn.microsoft.com/en-us/library/ms731082.aspx)
 * [Web API](https://www.asp.net/web-api)
 * [NancyFX](http://nancyfx.org/)


As most web technologies operate in a scale out manner NServiceBus is **usually** hosted in a "Send-only" manner. In this mode the given web application technology acts as a "forwarder" of messages rather than the "processor". So the handling code (MVC controller, NancyFX module etc) of a given web request simply leverages the  send APIs and no processing is done in the web process. The actual message handling is done in a [Windows Service Endpoint](windows-service.md).


## Endpoint Lifecycle

In a web hosted scenario an [IIS Recycle](https://msdn.microsoft.com/en-us/library/ms525803.aspx) causes the hosted NServiceBus endpoint to shutdown and be restarted.


## Container Integration

Web request handlers (MVC Controllers, WCF Handlers, NancyFx Modules, etc.) require access to the endpoint messaging session in order to send messages as a result of incoming HTTP requests. Many of the supported web application hosts resolve these web request handlers using an IoC container. NServiceBus already creates and manages it's own IoC container. 

It seems reasonable to re-use the same IoC container for both tasks. This will not work as the endpoint messaging session is not registered in the NServiceBus managed IoC container. This is to prevent NServiceBus message handlers from taking a dependency on the endpoint messaging session. See [Moving away from IBus - Dependency Injection](/nservicebus/upgrades/5to6/moving-away-from-ibus.md#dependency-injection) for more info. 

The recommended approach to handle this is to have two IoC containers: one for the web host and it's web request handlers, and another for the NServiceBus endpoint and it's message handlers. There are some things to consider when adopting this approach.

1. Any service which is registered with the NServiceBus IoC container will be available to be injected into NServiceBus message handlers but not web request handlers.
2. Any service which is registered with the web host IoC container will be available to be injected into web request handlers but not NServiceBus message handlers.
3. Any service which is registered with both containers can be resolved into both web request handlers and NServiceBus message bus handlers _but_:
	1. these will be different objects with different lifetimes.
	2. even if the services are registered with a singleton lifetime there will still be one created for each container
	3. if a service really needs to be shared and single instance, it must be created externally during the web application host startup, and that specific instance must be registered in both containers.
