---
title: Configuration API Queue Management in V3 and V4
summary: Configuration API Queue Management in V3 and V4
tags:
- NServiceBus
- BusConfiguration
- V3
- V4
---

At configuration time it is possible to define queue behavior:

* `PurgeOnStartup( Boolean purge )`: determines if endpoint queues should be purged at startup.
* `DoNotCreateQueues()`: configures the endpoint to not try to create queues at startup if they are not already created.