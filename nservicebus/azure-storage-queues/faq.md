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


### Do Azure Storage Queues provide an exactly once deliver model?

No, depending on the selected `TransportTransactionMode` it is either `at least once` or `at most once` delivery. Azure Storage Queues queues work with a so called 'queue-peek-lock' model to overcome the lack of transactions. When a worker pulls a message from a queue, it will actually become invisible instead of removed from the queue. The worker has to process the messages in a well defined timeframe and delete it explicitly when done. If the worker fails to do this, because it died, an exception was thrown or it was too slow, then the message will reappear on the queue and another instance of the worker can pick it up. This also implies that multiple workers may be working on the same message at the same time (esp. problematic when operations take to long). Depending a selected `TransportTransactionMode`, the transport deletes a message after processing (`at least once`) or before processing (`at most once`).