---
title: Scaling out MSMQ endpoints
summary: How to scale out when using the MSMQ transport
reviewed: 2020-06-16
component: MsmqTransport
related:
- nservicebus/messaging/routing
redirects:
 - nservicebus/msmq/scalability-and-ha
 - nservicebus/msmq/scaling-out
---


partial: overview

## MSMQ remote receive

Version 4 of MSMQ, made available with Vista and Server 2008, can perform [remote transactional receive](https://msdn.microsoft.com/en-us/library/ms700128.aspx). This means that processes on other machines can transactionally pull work from a queue on a different machine. If the machine processing the message crashes, the message rolls back to the queue and other machines could then process it.

One problem with 'remote transactional receive' is that it gets proportionally slower as more worker nodes are added. This is due to the overhead of managing more transactions, as well as the longer period of time that these transactions are open. In short, the scale-out benefits of MSMQ version 4 by itself are quite limited.
