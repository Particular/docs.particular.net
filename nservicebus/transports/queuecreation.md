---
title: Creating queues
summary: Explains how queues are created
component: Core
reviewed: 2016-08-25
versions: '[4,]'
tags:
 - Queue
 - QueueBindings
 - Queue creation
---

NServiceBus will automatically request the transport to create queues needed if the [installers](/nservicebus/operations/installers.md) are enabled.

This also includes queues needed by all declared [satellites](/nservicebus/satellites).

Refer to the scripting guidelines to take full control over queue creation:

* [SqlServer](/nservicebus/sqlserver/operations-scripting#create-queues)
* [MSMQ](/nservicebus/msmq/operations-scripting#create-queues)
* [RabbitMQ](/nservicebus/rabbitmq/operations-scripting#create-queues)
* [Azure ServiceBus](/nservicebus/azure-service-bus/operational-scripting)