---
title: Azure ServiceBus FAQ
summary: Frequently Asked Questions related to the Azure ServiceBus transport.
tags:
- Cloud
- Azure
- Transports
---

This document captures some frequently asked questions when using azure servicebus as a transport

### Does Azure ServiceBus provide the same consistency model as I'm used to with Msmq?

The short answer is no! The longer answer is: Queues are remote, instead of local, and this has several implications.

* A message has to cross the network boundaries before it is persisted, this implies that it is subject to all kinds network related issues like latency, timeouts, connection loss, network partitioning etc.
* Remote queues do not play along in transactions, as transactions are very brittle because of the possible network issues mentioned in the previous point, but also because they would require server side locks to function properly and allowing anyone to take unbound locks on a service is a very good way to get yourself in a denial of service situation. Hence Azure services typically don't allow transactions.


### Does Azure ServiceBus provide an exactly once deliver model?

By default it does not, it's an at least once delivery model. But you can enable a feature called Duplicate Detection, which will make sure you get a message exactly once, but this comes at the expense of throughput and is also limited by several conditions (time constrained, not available for partitioned entities).
