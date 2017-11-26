---
title: Endpoints multi hosting
summary: Hosting multiple endpoints in one process.
reviewed: 2017-11-15
component: Core
tags:
- Hosting
related:
- nservicebus/hosting
- nservicebus/hosting/assembly-scanning
---

## Code walk-through

This sample shows how to host multiple endpoints in one process. At start up the application creates two endpoint instances, connected to different queues using different configurations:

snippet: multi-hosting

One important thing to keep in mind is that internally [dependency injection](/nservicebus/dependency-injection/) is used to register components, handlers, and sagas; dependency injection is automatically configured at start up to scan all the assemblies found in the directory where the program is executed from. In order to ensure that each endpoint instance registers only its own components, it is important to specify an assembly scan policy using [one of the supported approaches](/nservicebus/hosting/assembly-scanning.md):

snippet: multi-hosting-assembly-scan

NOTE: This is possible only when self-hosting and not using NServiceBus.Host
