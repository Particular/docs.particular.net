---
title: Sending from a Nancy module
summary: Illustrates how to send messages to a NServiceBus endpoint from a Nancy web application.
component: Core
reviewed: 2018-01-04
related:
- nservicebus/dependency-injection
- nservicebus/hosting
- nservicebus/hosting/publishing-from-web-applications
---


### Initialize the Nancy web endpoint

Open `Bootstraper.cs` and look at the `ConfigureApplicationContainer` method.

snippet: ConfigureApplicationContainer


### Injection into the module

The endpoint instance is injected into the `SendMessageModule` at construction time.

snippet: Module