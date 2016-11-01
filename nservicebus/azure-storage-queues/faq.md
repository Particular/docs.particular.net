---
title: Frequently Asked Questions
component: ASQ
tags:
- Cloud
- Azure
- Transport
- ASQ
- Azure Storage Queues
reviewed: 2016-10-21
---


### Consistency model compared to MSMQ

include: azure-compared-to-msmq


### Exactly-once delivery model

No, they do not support an `exactly-once` delivery model. Depending on the selected `TransportTransactionMode` it is either `at-least-once` or `at-most-once` delivery. Azure Storage Queues work using a `Peek-Lock` model to overcome the lack of transactions. When an endpoint instance pulls a message from a queue, it is marked as invisible for a specified time. The endpoint instance has to process the messages in a well defined timeframe and delete it explicitly when done. When the message is not processed within the pre-defined time, a.k.a. Invisibility Timeout, the message will become visible on the queue and another endpoint instance can access it. Depending on the selected `TransportTransactionMode`, the transport deletes a message after processing (`at-least-once`) or before processing (`at-most-once`).