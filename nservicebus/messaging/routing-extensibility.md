---
title: Routing system extensibility points
summary: How NServiceBus routing can be extended
reviewed: 2020-03-30
component: Core
related:
 - nservicebus/messaging/routing
---

Extending the [NServiceBus routing subsystem](/nservicebus/messaging/routing.md) with a custom data source makes sense in following scenarios:

 * When centralizing all routing information in a database.
 * When dynamically calculating routes based on endpoint discovery protocol (similar to [UDDI](https://en.wikipedia.org/wiki/Web_Services_Discovery)).
 * When using a convention based on message naming.

partial: content 

partial: endpoint-mappings