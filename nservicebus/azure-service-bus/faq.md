---
title: Azure Service Bus Transport FAQ
summary: Frequently Asked Questions related to the Azure Service Bus Transport.
tags:
- Cloud
- Azure
- Transports
---


### Does Azure Service Bus provide the same consistency model as MSMQ?

include: azure-compared-to-msmq


### Does Azure Service Bus provide an exactly once deliver model?

By default it does not, it's an at least once delivery model. A feature called Duplicate Detection can be enabled, which will ensure the message is received exactly once, but this comes at the expense of throughput and is also limited by several conditions (time constrained, not available for partitioned entities).
