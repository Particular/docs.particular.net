---
title: Outbox with RavenDB persistence
component: Raven
reviewed: 2024-09-26
versions: '[2.0,)'
related:
- nservicebus/outbox
redirects:
- nservicebus/ravendb/outbox
---

include: dtc-warning

include: cluster-configuration-info

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store messages and enable deduplication.

## Extra collections created by the RavenDB Outbox persistence

To keep track of duplicate messages, the RavenDB implementation of Outbox creates a special collection of documents called `OutboxRecord`.

All endpoints connected to the same database will store outbox records in this collection with the key `Outbox/{Endpoint-name}/{Message-id}`

### Overriding endpoint name

TODO: Version this part and pull code into a snippet
To override the endpoint name use:

```
var outboxSettings = endpointConfiguration.EnableOutbox();
outboxSettings.EndpointName("MyEndpoint");
```

## Deduplication record lifespan

partial: cleanup
