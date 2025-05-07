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

## Storage format

The persister stores outbox data for all endpoints in a [collection](https://ravendb.net/docs/article-page/7.0/csharp/client-api/faq/what-is-a-collection) called `OutboxRecords`. To separate data for individual endpoints stored documents will have their endpoint name embedded in the document ID using the following format `Outbox/{Endpoint-name}/{Message-id}`.

### Overriding endpoint name

TODO: Version this part and pull code into a snippet

To enable scenarios like being a [processor endpoint for the transactional session](/nservicebus/transactional-session/index.md#remote-processor) the endpoint name used can be configured:

```
var outboxSettings = endpointConfiguration.EnableOutbox();
outboxSettings.EndpointName("MyEndpoint");
```

## Deduplication record lifespan

partial: cleanup
