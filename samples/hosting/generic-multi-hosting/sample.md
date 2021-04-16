---
title: Generic Host multiple endpoint hosting
summary: Hosting multiple endpoints in a generic host process.
reviewed: 2020-08-10
component: Core
related:
- nservicebus/hosting
redirects:
- samples/hosting/multi-hosting
---

## Code walk-through

This sample shows how to host multiple endpoints in one generic host process by using multiple `IHostBuilder` instances. When started, the application creates two host builder instances, each configured for a different endpoint that could be using a different configurations:

snippet: multi-hosting-startup

An important thing to keep in mind is that [dependency injection](/nservicebus/dependency-injection/) is used internally to register components, handlers, and sagas. Each host has a separate ServiceProvider which means the containers are not shared between the endpoints. 

To ensure that each endpoint instance registers only its own components like message handlers, it is important to specify an assembly scan policy using [one of the supported approaches](/nservicebus/hosting/assembly-scanning.md).

WARN: If a single endpoint fails to start, the host will shut down, terminating the other endpoint.

In this example, complete isolation is required between the two endpoints so that the types from Instance2 are excluded from Instance1 and vice versa.

snippet: multi-hosting-assembly-scan
