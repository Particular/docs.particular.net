---
title: Generic Host multiple endpoint hosting
summary: Hosting multiple endpoints in a generic host process.
reviewed: 2020-08-10
component: Core
related:
- nservicebus/hosting
redirect:
- samples/hosting/multi-hosting
---

## Code walk-through

This sample shows how to host multiple endpoints in one Generic Host process by using multiple `IHostBuilder` instances. When started the application creates two host builder instances, each configured for a different endpoint that could be using a different configurations:

snippet: multi-hosting-startup

One important thing to keep in mind is that [dependency injection](/nservicebus/dependency-injection/) is used internally to register components, handlers, and sagas. Each host has a separate ServiceProvider which means the containers are not shared between the endpoints. 

In order to ensure that each endpoint instance registers only its own components like message handlers, it is important to specify an assembly scan policy using [one of the supported approaches](/nservicebus/hosting/assembly-scanning.md). 

In this example, complete isolation is required between the two endpoints, so from Instance1 the types from Instance2 are excluded and vice versa.

snippet: multi-hosting-assembly-scan
