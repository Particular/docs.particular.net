---
title: Sending from an NANCY Module
summary: Illustrates how to send messages to a NServiceBus endpoint from a NANCY Web application.
component: Core
reviewed: 2017-12-09
related:
- nservicebus/dependency-injection
- nservicebus/hosting
- nservicebus/hosting/publishing-from-web-applications
---


### Initialize the NANCY Web endpoint

Open `Bootstraper.cs` and look at the `ConfigureApplicationContainer` method.

snippet: ConfigureApplicationContainer


### Injection into the Module

The endpoint instance is injected into the `SendMessageModule` at construction time.

snippet: Module