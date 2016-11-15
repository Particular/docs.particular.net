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
---

MSMQ is a [bus transport](/nservicebus/transports). This means that an MSMQ system consists of multiple nodes, one on each machine, forwarding messages between each other. Each endpoint connects to its local MSMQ node. In order to address a different endpoint, not only the queue name but also the host name has to be specified.


## Scaling out

Because the MSMQ queues are not accessible from outside the machine they are hosted at, NServiceBus endpoints using the MSMQ transport are unable to use the competing consumers pattern to scale out with a single shared queue. 

partial:scale-out


## Physical mapping

partial:content

