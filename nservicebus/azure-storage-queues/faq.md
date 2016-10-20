---
title: Frequently Asked Questions
component: ASQ
tags:
- Cloud
- Azure
- Transport
- ASQ
- Azure Storage Queues
---


### Do Azure Storage Queues provide the same consistency model as MSMQ?

include: azure-compared-to-msmq


### Do Azure Storage Queues provide an exactly-once deliver model?

No, they do not support an exactly-once deliver model. Depending on the selected `TransportTransactionMode` it is either `at-least-once` or `at-most-once` delivery. Azure Storage Queues queues work with a so called Peek-Lock model to overcome the lack of transactions. When a endpoint instance pulls a message from a queue, it is marked as invisible for specified period of time. The endpoint instance has to process the messages in a well defined timeframe and delete it explicitly when done. When the message is not processed within the pre-defined time, a.k.a. Invisibility Timeout, the message will become visible on the queue and another endpoint instance can access it. Depending on a selected `TransportTransactionMode`, the transport deletes a message after processing (`at least once`) or before processing (`at most once`).