---
title: Property injection
summary: How to configure property injection
tags:
- Dependency Injection
- IOC
redirects:
 - nservicebus/property-injection-in-handlers
---

WARNING: As of Version 6 property injection is no longer supported via the various NServiceBus configuration API's. Should you still need to use property injection we recommend that switch to one of our [supported containers](/nservicebus/containers) and configure property injection using the native configuration API of the selected container.

When using the NServiceBus built-in container it is possible to control property injection.

So given the following class that is constructed by the container.

snippet: PropertyInjectionWithHandler

The inject property values you could do the following:

snippet: ConfigurePropertyInjectionForHandlerBefore

At construction time both `SmtpAddress` and `SmtpPort` will be injected.


## A Handler/Saga specific API

From Version 5.2 and above a new, more explicit, API has been introduced that specifically targets Handlers and Sagas.

snippet: ConfigurePropertyInjectionForHandler
