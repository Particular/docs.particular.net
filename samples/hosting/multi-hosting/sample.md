---
title: NServiceBus multi hosting
summary: This sample shows how to host multiple NServiceBus instances in one process.
tags:
- Hosting
---

## Code walk-through

This sample shows how to host multiple NServiceBus instances in one process. At start up the application creates to `IBus` instances, connected to different queues using a different configuration:

<!-- import multi-hosting -->

One important thing to keep in mind is that NServiceBus internally utilizes an [Inversion of Control](/nservicebus/containers/) container to register all its components as well as user-implemented handlers, sagas and components; the container is automatically configured at start up scanning all the assemblies found in the directory where the program is executed from, in order to enforce that each bus instance registers only components its own components it is important to specify an assembly scan policy using [one of the supported approaches](/nservicebus/hosting/assembly-scanning.md) such as in the following sample: 

<!-- import multi-hosting-assembly-scan -->
