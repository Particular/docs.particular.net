---
title: Interface lifecycles
summary: The lifecycles of the various NServiceBus configuration interfaces
reviewed: 2024-11-19
redirects:
- samples/plugin-based-config
---

Each endpoint instance goes through a series of events as it is configured, constructed, started, and stopped. NServiceBus provides extension points that allow to execute code at specific stages of the instance lifecycle. 

The most common lifecycle extension points include:

- [Initialization](/nservicebus/lifecycle/ineedinitialization.md): Called when the endpoint is first created. Extensions added to this part of the lifecycle typically contribute to the configuration of the endpoint.
- [Before Configuration Finalized](/nservicebus/lifecycle/iwanttorunbeforeconfigurationisfinalized.md): Called just before the configuration is made read-only and the endpoint is started. Extensions added to this part of the lifecycle are usually last minute checks or tweaks to other parts of endpoint configuration.
- [Endpoint Instance Started/Stopped](/nservicebus/lifecycle/endpointstartandstop.md): This article describes approaches to running tasks around the endpoint startup and shutdown lifecycle events.
