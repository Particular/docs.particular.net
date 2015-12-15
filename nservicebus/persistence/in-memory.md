---
title: In Memory Persistence
summary: In memory persistence stores data in a non-durable manner.
tags:
- Persistence
---

Some scenarios require an in-memory persistence such as the development environment or a lightweight client not interested in durability across restarts:

snippet:ConfiguringInMemory

WARNING: Because there is no clustering / replication happening between NServiceBus instances, when a process is down, all the information stored in the InMemory persistence of that process is lost.

NOTE: The [Second Level Retry (SLR)](/nservicebus/errors/automatic-retries.md) mechanism uses the deferred delivery (also known as *timeouts*) feature internally. As SLR is enabled by default, using InMemory persistence means you are risking losing messages that have failed processing and are waiting for another retry attempt. Use InMemory persistence only is scenarios where it is OK to lose messages.
