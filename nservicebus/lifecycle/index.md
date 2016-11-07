---
title: Interface life-cycles
summary: The life-cycles of the various NServiceBus configuration interfaces
reviewed: 2016-11-07
---

Each Endpoint Instance goes through a series of events as it is configured, constructed, started and stoppped. NServiceBus provides extension points that allow to execute code at specific stages of the Instance life-cycle. 

The most common life-cycle extension points include:

- [Initialization](/nservicebus/lifecycle/ineedinitialization.md): Called when the endpoint is first created. Extensions added to this part of the life-cycle typically contribute to the configuration of the endpoint.
- [Before Configuration Finalized](/nservicebus/lifecycle/iwanttorunbeforeconfigurationisfinalized.md): Called just before the configuration is made read-only and the endpoint is started. Extensions added to this part of the life-cycle are usually last minute checks or tweaks to other parts of endpoint configuration.
- [Endpoint Instance Started/Stopped](/nservicebus/lifecycle/endpointstartandstop.md): Called after the endpoint has been started and before it is stopped. In Version 6 and above, these extension points have been moved to the hosts. For self-hosted endpoints, call the desired code directly after starting and before stopping the endpoint rather than relying on explicit NServiceBus extension points.
