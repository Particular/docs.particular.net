---
title: MSMQ Information
summary: MSMQ is the primary durable communications technology for Microsoft but does not dynamically detect network interfaces.
tags: []
redirects:
 - nservicebus/msmq-information
---

MSMQ is the default transport used by NServiceBus.


## NServiceBus Configuration

NServiceBus requires a specific MSMQ configuration to run. To enable this configuration either use "NServiceBus Prerequisites" in the [Platform Installer](/platform/installer/) or using the [Powershell Module](/nservicebus/operations/management-using-powershell.md).


## MSMQ clustering

MSMQ clustering works by having the active node running the instance of the MSMQ service and the other nodes being cold standbys. On failover, a new instance of the MSMQ service has to be loaded from scratch. All active network connections and associated queue handles break and have to be reconnected. Any transactional processing of messages aborts, returning the message to the queue after startup.

So downtime is proportional to the time taken for the MSMQ service to restart on another node. This is affected by how many messages are in currently storage, awaiting processing.


## Remote Queues

Remote queues are not supported for MSMQ as this conflicts with the Distributed Bus architectural style that is predicated on consents of durability, autonomy and avoiding a single point of failure.
For scenarios where you want to use a Broker Bus style architecture you can use transports like Sql Server and RabbitMQ.
