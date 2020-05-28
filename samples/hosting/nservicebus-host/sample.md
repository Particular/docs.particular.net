---
title: The NServiceBus.Host
summary: Hosting an NServiceBus endpoint using the NServiceBus.Host.
reviewed: 2019-11-25
component: Host
related:
- nservicebus/hosting/nservicebus-host
- nservicebus/lifecycle
---

include: host-deprecated-warning

## Code walk-through

This sample shows how to host an NServiceBus endpoint using the NServiceBus.Host.

snippet: nservicebus-host


## Running code at start and stop time

Since the configuration is done via an implementation of `IConfigureThisEndpoint` and the host controls the startup process there is no regular way to run code at startup and stop of the endpoint. To enable this scenario, there is an interface that is assembly scanned and executed at these times.

snippet: RunWhenStartsAndStops