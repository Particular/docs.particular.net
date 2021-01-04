---
title: Creating queues
summary: Explains how queues are created
component: Core
reviewed: 2021-01-04
versions: '[4,]'
redirects:
- nservicebus/queuecreation
---


WARNING: NServiceBus will automatically request the transport to create queues needed if the [installers](/nservicebus/operations/installers.md) are enabled. This also includes queues needed by all declared [satellites](/nservicebus/satellites). Prefer the use of scripts to create custom queues instead of relying on interfaces provided by NServiceBus.


The scripting guidelines shows how to take full control over queue creation:

 * [SQLServer](/transports/sql/operations-scripting.md#create-queues)
 * [MSMQ](/transports/msmq/operations-scripting.md#create-queues)
 * [RabbitMQ](/transports/rabbitmq/operations-scripting.md#create-queues)
 * [Azure ServiceBus](/transports/azure-service-bus/operational-scripting.md)
 

## Declaration

Queues should be declared during the Setup phase of a [Feature](/nservicebus/pipeline/features.md).

A built-in example is the audit feature which needs the audit queue. During start-up NServiceBus ensures the declared queues are present and aborts the start-up procedure if they are not (with an exception of MSMQ remote queues which are optional).

snippet: queuebindings

## Creation

Queues are created only during [installation](/nservicebus/operations/installers.md).
