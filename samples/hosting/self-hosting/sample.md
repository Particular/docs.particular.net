---
title: Self hosting
summary: Hosting in-process Endpoints
reviewed: 2017-11-15
component: Core
tags:
- Hosting
related:
- nservicebus/hosting
---

## Code walk-through

This sample shows how to host an in-process instance of NServiceBus.

snippet: self-hosting

WARNING: Although not shown in this sample, when self-hosting NServiceBus, the [critical error action](/nservicebus/hosting/critical-errors.md) should always be overridden. If a critical error occurs, NServiceBus will stop the endpoint but will not shut down the process. By specifying a critical error action, the host application can elect to terminate in order to be respawned, or take action to notify system administrators of the failure.
