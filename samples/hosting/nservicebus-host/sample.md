---
title: The NServiceBus.Host
summary: Hosting a NServiceBus endpoint using the NServiceBus.Host.
reviewed: 2016-04-06
component: Host
tags:
- Hosting
related:
- nservicebus/hosting/nservicebus-host
- nservicebus/lifecycle
---


## Code walk-through

This sample shows how to host a NServiceBus endpoint using the NServiceBus.Host.

snippet: nservicebus-host


## Running code when at start and stop

Since the configuration is done via an implementation of `IConfigureThisEndpoint` and the host controls the startup process there is no regular way to run code at startup and stop of the endpoint. To enable this scenario there is an interface that is assembly scanned and executed at these times.

snippet: RunWhenStartsAndStops
