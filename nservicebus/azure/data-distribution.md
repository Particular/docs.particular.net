---
title: Data distribution
summary: How to configure NServiceBus to distribute data to all instances of a given endpoint
component: Core
reviewed: 2016-08-10
versions: "[5,)"
tags:
- Azure
- Transport
redirects:
- nservicebus/scalability-and-ha/individualizing-queues-when-scaling-out
---

DANGER: Asynchronous messaging is not an optimal solution for data distribution scenarios. It is usually better to use a dedicated data distribution technology for that purpose, such as a distributed cache or distributed configuration service.

NServiceBus [Publish/Subscribe](/nservicebus/messaging/publish-subscribe) mechanism always delivers events to a single instance of each logical subscriber. Sometimes, however, there is a reason for delivering each event to all instances of the subscriber in order to distribute data. While it is usually better to use a dedicated data distribution technology for that purpose (e.g. a distributed caching solution), NServiceBus can also be used in data-distribution mode.

partial: content 
