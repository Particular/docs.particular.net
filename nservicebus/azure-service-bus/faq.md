---
title: Frequently Asked Questions
reviewed: 2016-09-14
tags:
 - Cloud
 - Azure
 - Transport
---


### Consistency model compared to MSMQ

include: azure-compared-to-msmq


### Exactly-once delivery model

By default it does not, it's an at least once delivery model. A feature called [Duplicate Detection](https://msdn.microsoft.com/library/azure/microsoft.servicebus.messaging.queuedescription.requiresduplicatedetection.aspx) can be enabled, which will ensure the message is received exactly once. However, Duplicate Detection comes at the expense of throughput and is limited by several conditions. For example it is time constrained and not available for partitioned entities.