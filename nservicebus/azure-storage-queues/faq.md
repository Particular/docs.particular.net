---
title: Azure Storage Queues Transport FAQ
summary: Frequently Asked Questions related to using azure storage queues as a transport.
tags:
- Cloud
- Azure
- Transport
- ASQ
- Azure Storage Queues
---


### Do Azure Storage Queues provide the same consistency model as MSMQ?

include: azure-compared-to-msmq


### Do Azure Storage Queues provide an exactly once deliver model?

No, it's at least once delivery. To overcome the lack of transactions, these queues work with a so called 'queue-peek-lock' model. When a worker pulls a message from a queue, it will actually become invisible instead of removed from the queue. The worker has to process the messages in a well defined timeframe and delete it explicitly when done. If the worker fails to do this, because it died, an exception was thrown or it was too slow, then the message will reappear on the queue and another instance of the worker can pick it up. This also implies that multiple workers may be working on the same message at the same time (esp. problematic when operations take to long).