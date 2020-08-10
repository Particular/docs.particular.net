---
title: Generic Host multiple endpoint hosting
summary: Hosting multiple endpoints in a generic host process.
reviewed: 2020-08-10
component: Core
related:
- nservicebus/hosting
---

## Code walk-through

This sample shows how to host multiple endpoints in one Generic Host process by using multiple `IHostBuilder` instances. When started the application creates two host builder instances, each configured for a different endpoint that could be using a different configurations:

One important thing to keep in mind is that [dependency injection](/nservicebus/dependency-injection/) is used internally to register components, handlers, and sagas. Dependency injection is automatically configured during endpoint start up to scan all the assemblies found in the directory where the program is executed from. In order to ensure that each endpoint instance registers only its own components, it is important to specify an assembly scan policy using [one of the supported approaches](/nservicebus/hosting/assembly-scanning.md).