---
title: Frequently Asked Questions
reviewed: 2017-05-05
component: ASB
tags:
 - Azure
 - Transport
redirects:
 - nservicebus/azure-service-bus/faq
---


### Consistency model compared to MSMQ

include: azure-compared-to-msmq


### Exactly-once delivery model

By default it does not, it's an **`at-least-once`** delivery model. A feature called [Duplicate Detection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.queuedescription#Microsoft_ServiceBus_Messaging_QueueDescription_RequiresDuplicateDetection) can be enabled, which will ensure the message is received exactly once. However, Duplicate Detection comes at the expense of throughput and is time constrained.
