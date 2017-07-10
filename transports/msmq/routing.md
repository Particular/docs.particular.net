---
title: Physical routing with MSMQ
summary: Configuring physical routing with MSMQ transport
component: MsmqTransport
reviewed: 2016-10-26
tags:
 - Routing
 - MSMQ
related:
 - nservicebus/messaging/routing
redirects:
 - nservicebus/msmq/routing
---

The MSMQ transport in NServiceBus is a distributed transport in which there is the [MSMQ process](https://msdn.microsoft.com/en-us/library/ms711472.aspx) running on each machine, with messages being stored locally before being forwarded to other machines. In this model, each `endpoint` connects to the local MSMQ process and, when addressing a different endpoint, not only the queue name but also the host name has to be specified.


## Scaling out

Because the MSMQ queues are not accessible from outside the machine they are hosted at, NServiceBus endpoints using the MSMQ transport are unable to use the competing consumers pattern to scale out with a single shared queue. 

partial: scale-out


## Physical mapping

partial: content

