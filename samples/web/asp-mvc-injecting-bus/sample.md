---
title: Injecting the Bus into ASP.NET MVC Controller
summary: Leverages Autofac MVC integration to inject IBus into MVC Controllers.
tags: []
redirects:
- nservicebus/injecting-the-bus-into-asp.net-mvc-controller
related:
- nservicebus/containers
- nservicebus/hosting
- nservicebus/hosting/publishing-from-web-applications
---


### Wire up Autofac

Open `Global.asax.cs` and look at the `ApplicationStart` method.

snippet:ApplicationStart


### Injection into the Controller

Note that `IEndpointInstance` (in version 6) or `IBus` (in version 5) is injected into the `DefaultController` at construction time.

snippet:Controller
