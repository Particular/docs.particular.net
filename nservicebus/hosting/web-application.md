---
title: Web Application Hosting
summary: Hosting NServiceBus in a Web Site or Web Service
reviewed: 2016-05-16
tags:
- Hosting
- Web Site
- Web Service
related:
 - samples/web
 - nservicebus/lifecycle
 - samples/startup-shutdown-sequence
 - samples/endpoint-configuration
 - nservicebus/messaging/callbacks
 - nservicebus/hosting/publishing-from-web-applications
---


NServiceBus can be hosted in any web technology that support .NET. This includes, but not limited to, the following:

 * [ASP.net](http://www.asp.net/get-started/websites)
 * [ASP.MVC](http://www.asp.net/mvc)
 * [Windows Communication Foundation (WCF)](https://msdn.microsoft.com/en-us/library/ms731082.aspx)
 * [Web API](http://www.asp.net/web-api)
 * [NancyFX](http://nancyfx.org/)

As most web technologies operate in a scale out manner NServiceBus is **usually** hosted in a "Send-only" manner. In this mode the give web application technology acts as a "forwarder" of messages rather than the "processor". So the handling code (MVC controller, NancyFX module etc) of a given web request simply leverages the  send APIs and no processing is done in the web process. The actual message handling is done in a [Windows Service Endpoint](windows-service.md).

NOTE: In a web hosted scenario a [IIS Recycle](https://msdn.microsoft.com/en-us/library/ms525803.aspx) is considered a shutdown and restart of the bus.