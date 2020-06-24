---
title: Physical routing with MSMQ
summary: Configuring physical routing with MSMQ transport
component: MsmqTransport
reviewed: 2020-05-20
related:
 - nservicebus/messaging/routing
 - transports/msmq/routing-extensibility
redirects:
 - nservicebus/msmq/routing
---

The MSMQ transport in NServiceBus is a distributed transport in which the [MSMQ process](https://docs.microsoft.com/en-us/previous-versions/windows/desktop/legacy/ms711472(v=vs.85)) runs on each machine, storing messages locally before being forwarded to other machines. In this model, each `endpoint` connects to the local MSMQ process and both the queue name and the host name must be specified when addressing a different endpoint.

## Scaling out

Because the MSMQ queues are not accessible from outside the machine they are hosted in, NServiceBus endpoints using the MSMQ transport are not able to use the competing consumers pattern to scale out with a single shared queue. 

partial: scale-out

## Physical mapping

partial: content

partial: schema-validation