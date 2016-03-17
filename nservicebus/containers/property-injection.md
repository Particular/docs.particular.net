---
title: Property injection
summary: How to configure property injection
reviewed: 2016-03-17
tags:
- Dependency Injection
- IOC
redirects:
 - nservicebus/property-injection-in-handlers
---

NServiceBus will automatically enable property injection known types across the supported IOC Containers. Use the lambda overload of `.ConfigureComponent` to get full control over the injected properties if needed.

A common use case it to set primitive properties on message handlers. Given then below handler:

snippet: PropertyInjectionWithHandler

Setting the properties would be done as follows:

snippet: ConfigurePropertyInjectionForHandler

## A Handler/Saga specific API

Versions 5.2 supported a new, more explicit, API that specifically targets Handlers and Sagas.

NOTE: This api has been obsoleted in Version 6

snippet: ConfigurePropertyInjectionForHandlerExplicit
