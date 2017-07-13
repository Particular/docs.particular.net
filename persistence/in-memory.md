---
title: In-Memory Persistence
summary: In-Memory persistence stores data in a non-durable manner for development-time only
component: InMemoryPersistence
reviewed: 2017-07-12
redirects:
- nservicebus/persistence/in-memory
tags:
- Persistence
---

Some scenarios require an in-memory persistence such as the development environment or a lightweight client not interested in durability across restarts:

snippet: ConfiguringInMemory

DANGER: All information stored in the In-Memory persistence is discarded when the process ends.

NOTE: The [Delayed Retries](/nservicebus/recoverability/#delayed-retries) mechanism uses the [timeout manager](/nservicebus/messaging/timeout-manager.md) when a transport does not natively support delayed delivery. As Delayed Retries are enabled by default, using In-Memory persistence with a transport that uses the timeout manager has the risk of losing messages that have failed processing and are waiting for another retry attempt. Use In-Memory persistence only in scenarios where it is OK to lose messages.
