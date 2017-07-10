---
title: In Memory Persistence
summary: In memory persistence stores data in a non-durable manner for development time only
component: InMemoryPersistence
reviewed: 2016-08-24
redirects:
- nservicebus/persistence/in-memory
tags:
- Persistence
---

Some scenarios require an in-memory persistence such as the development environment or a lightweight client not interested in durability across restarts:

snippet: ConfiguringInMemory

DANGER: All information stored in the InMemory persistence is discarded when the process ends.

NOTE: The [Delayed Retries](/nservicebus/recoverability/#delayed-retries) mechanism uses the deferred delivery (also known as *timeouts*) feature internally. As Delayed Retries is enabled by default, using InMemory persistence means the risk of losing messages that have failed processing and are waiting for another retry attempt. Use InMemory persistence only is scenarios where it is OK to lose messages.
