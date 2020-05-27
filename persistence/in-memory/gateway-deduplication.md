---
title: In-memory gateway deduplication persistence
summary: In-memory gateway persistence stores data in a non-durable manner for development-time only
component: Gateway
versions: '[3.1,)'
reviewed: 2019-12-17
---

DANGER: All information stored is discarded when the process ends. This can result in more-than-once message delivery.

The in-memory gateway deduplication persistence uses a least-recently-used (LRU) cache. By default this cache can contain up to 10,000 items. The maximum size can be changed using the following API:

snippet: InMemoryDeduplicationConfigurationCacheSize