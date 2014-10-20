---
title: Configuration API InMemory Persistence in V5
summary: Configuration API InMemory Persistence in V5
tags:
- NServiceBus
- BusConfiguration
- V5
- Persistence
---

Some scenarios my benefit from an in-memory persistence configuration, such as the development environment or a lightweight client not interested in durability across restarts. In order to configure in-memory persistence use the `InMemoryPersistence` persistence class.

Details of all the persistence options are in the [Persistence in NServiceBus](/nservicebus/persistence-in-nservicebus) article.