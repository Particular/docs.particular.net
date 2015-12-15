---
title: Property injection
summary: How to configure property injection
tags: 
- Dependency Injection
- IOC
redirects:
 - nservicebus/property-injection-in-handlers
---

When using the NServiceBus built-in container it is possible to control property injection.

So given the following class that is constructed by the container.

<!-- import PropertyInjectionWithHandler --> 

The inject property values you could do the following:

<!-- import ConfigurePropertyInjectionForHandlerBefore --> 

At construction time both `SmtpAddress` and `SmtpPort` will be injected.


## A Handler/Saga specific API

From Version 5.2 and above a new, more explicit, API has been introduced that specifically targets Handlers and Sagas.

<!-- import ConfigurePropertyInjectionForHandler --> 
