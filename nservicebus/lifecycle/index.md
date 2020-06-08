---
title: Interface lifecycles
summary: The lifecycles of the various NServiceBus configuration interfaces
reviewed: 2020-06-08
---

Each endpoint instance goes through a series of events as it is configured, constructed, started, and stopped. NServiceBus provides extension points that allow to execute code at specific stages of the instance lifecycle. 

The most common lifecycle extension points include:

- [Initialization](/nservicebus/lifecycle/ineedinitialization.md): Called when the endpoint is first created. Extensions added to this part of the lifecycle typically contribute to the configuration of the endpoint.
- [Before Configuration Finalized](/nservicebus/lifecycle/iwanttorunbeforeconfigurationisfinalized.md): Called just before the configuration is made read-only and the endpoint is started. Extensions added to this part of the lifecycle are usually last minute checks or tweaks to other parts of endpoint configuration.
- [Endpoint Instance Started/Stopped](/nservicebus/lifecycle/endpointstartandstop.md): Called after the endpoint has been started and before it is stopped. In NServiceBus version 6 and above, these extension points have been moved to the hosts. For self-hosted endpoints, call the desired code directly after starting and before stopping the endpoint rather than relying on explicit NServiceBus extension points.
