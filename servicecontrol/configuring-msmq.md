---
title: Configuring MSMQ for use with ServiceControl
summary: How to configure MSMQ to feed data into ServiceControl
component: MSMQTransport
reviewed: 2021-02-17
versions: '[5,)'
related:
 - nservicebus/recoverability/configure-error-handling
---

ServiceControl reads messages from the input queue specified in the [ServiceControl instance transport configuration](creating-config-file.md#transport). Monitored endpoints send appropriate messages, e.g. error or heartbeats, to the configured queues. ServiceControl ingests those messages in order to extract required information and saves messages in internal storage. 

Depending on the selected transport and the deployment scenario, the production endpoints might require additional configuration for ServiceControl. Currently only MSMQ transport requires it.

The following transports don't need any additional configuration for remote queues:

 * SQL Server
 * RabbitMQ
 * Azure Storage Queues
 * Azure Service Bus
 * Amazon SQS

See [Configure error handling](/nservicebus/recoverability/configure-error-handling.md) for examples.


## Remote MSMQ queues

If ServiceControl is installed on a different machine than endpoints using MSMQ Transport, then endpoints must be configured to send error and audit messages to a remote queue:

partial: snippets

For more information about how to configure the audit queue refer to the [Configuring auditing](/nservicebus/operations/auditing.md#configuring-auditing) documentation.
