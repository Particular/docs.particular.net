---
title: Endpoints multi hosting
summary: Hosting multiple endpoints in one process.
reviewed: 2019-09-16
component: Core
related:
- nservicebus/hosting
- nservicebus/hosting/assembly-scanning
---

## Code walk-through

This sample shows how to host multiple endpoints in one process. When started the application creates two endpoint instances, each connected to different queues using different configurations:

snippet: multi-hosting

One important thing to keep in mind is that [dependency injection](/nservicebus/dependency-injection/) is used internally to register components, handlers, and sagas. Dependency injection is automatically configured during endpoint start up to scan all the assemblies found in the directory where the program is executed from. In order to ensure that each endpoint instance registers only its own components, it is important to specify an assembly scan policy using [one of the supported approaches](/nservicebus/hosting/assembly-scanning.md):

snippet: multi-hosting-assembly-scan

NOTE: This is possible only when self-hosting and not using NServiceBus.Host