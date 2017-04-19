---
title: Scaling out MSMQ endpoints
summary: How to scale out when using the MSMQ transport.
reviewed: 2016-11-07
component: MsmqTransport
tags:
- scalability
- Routing
- MSMQ
related:
- nservicebus/messaging/routing
redirects:
- nservicebus/msmq/scalability-and-ha
---


partial: overview

## MSMQ remote receive

Version 4 of MSMQ, made available with Vista and Server 2008, can perform [remote transactional receive](https://msdn.microsoft.com/en-us/library/ms700128.aspx). This means that processes on other machines can transactionally pull work from a queue on a different machine. If the machine processing the message crashes, the message roll back to the queue and other machines could then process it.

The biggest problem with 'remote transactional receive' is that it gets proportionally slower as more worker nodes are added. This is due to the overhead of managing more transactions, as well as the longer period of time that these transactions are open. In short, the scale out benefits of MSMQ Version 4 by itself are quite limited and we don't recommend it.
