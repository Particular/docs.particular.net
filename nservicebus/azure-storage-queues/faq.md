---
title: Azure Storage Queues FAQ
summary: Frequently Asked Questions related to using azure storage queues as a transport.
tags:
- Cloud
- Azure
- Transports
---

### Do Azure Storage Queues provide the same consistency model as I'm used to with Msmq?

The short answer is no! The longer answer is: Queues are remote, instead of local, and this has several implications.

* A message has to cross the network boundaries before it is persisted, this implies that it is subject to all kinds network related issues like latency, timeouts, connection loss, network partitioning etc.
* Remote queues do not play along in transactions, as transactions are very brittle because of the possible network issues mentioned in the previous point, but also because they would require server side locks to function properly and allowing anyone to take unbound locks on a service is a very good way to get yourself in a denial of service situation. Hence Azure services typically don't allow transactions.


### Do Azure Storage Queues provide an exactly once deliver model?

No, it's at least once delivery. To overcome the lack of transactions, these queues work with a so called 'queue-peek-lock' model. When a worker pulls a message from a queue, it will actually become invisible instead of removed from the queue. The worker has to process the messages in a well defined timeframe and delete it explicitly when done. If the worker fails to do this, because it died, an exception was thrown or it was simply to slow, then the message will reappear on the queue and another instance of the worker can pick it up. This also implies that multiple workers may be working on the same message at the same time (esp. problematic when operations take to long)
