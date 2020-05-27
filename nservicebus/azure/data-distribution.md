---
title: Data distribution
summary: How to configure NServiceBus to distribute data to all instances of a given endpoint
component: Core
reviewed: 2018-12-06
versions: "[5,)"
redirects:
- nservicebus/scalability-and-ha/individualizing-queues-when-scaling-out
---

DANGER: Asynchronous messaging is not an optimal solution for data distribution scenarios. It is usually better to use a dedicated data distribution technology for that purpose, such as a distributed cache or distributed configuration service.

The NServiceBus [Publish-Subscribe](/nservicebus/messaging/publish-subscribe) implementation is designed to deliver events to a single physical instance of a logical subscriber. In order to distribute data, it may be necessary to deliver events to _all_ physical instances of a logical subscriber. While it is usually better to use a dedicated data distribution technology for that purpose, such as a distributed caching solution, NServiceBus can also be used to distribute data.

partial: content 