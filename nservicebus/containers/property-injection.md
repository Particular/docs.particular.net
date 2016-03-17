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

When using the NServiceBus built-in container it is possible to control property injection.

So given the following class that is constructed by the container.

snippet: PropertyInjectionWithHandler

The property inject be done with the following:

snippet: ConfigurePropertyInjectionForHandlerBefore

At construction time both `SmtpAddress` and `SmtpPort` will be injected.


## A Handler/Saga specific API

From Versions 5.2 and above a new, more explicit, API has been introduced that specifically targets Handlers and Sagas.

snippet: ConfigurePropertyInjectionForHandler