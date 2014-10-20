---
title: Configuration API Queue Management in V5
summary: Configuration API Queue Management in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

At configuration time it is possible to define queue behavior:

* `PurgeOnStartup( Boolean purge )`: determines if endpoint queues should be purged at startup. Purging queue at startup means that messages in the queue will be deleted each time the endpoint starts;
* `DoNotCreateQueues()`: configures the endpoint to not try to create queues at startup if they are not already created.