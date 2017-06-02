---
title: Configuring production queues for use with ServiceControl
summary: How to configure production queues to feed data into ServiceControl
reviewed: 2017-05-26
related:
 - nservicebus/recoverability/configure-error-handling
---

ServiceControl reads messages from the input queue specified in the [ServiceControl instance transport configuration](creating-config-file#transport). The queues specified during this stage are populated with messages from each of the production endpoints after which, ServiceControl ingests those messages in order to extract the required information from them. Depending on the transport selected and the deployment scenario, the production endpoints might need to be configured to send the messages to a remote queue.

Of the currently supported transports, only MSMQ requires special configuration to deal with remote queues.

### MSMQ

If the endpoints are on a separate machine to the ServiceControl instance, the endpoints must be configured to send their error and audit messages (if auditing is enabled) to a remote queue as the MSMQ instance mapping file is not used in the case of error and audit queues

Example configurations for a remote machine follow:

partial: ConfigMsmqErrorQueueDefault

partial: ConfigMsmqErrorWithCode

include: configurationWarning

partial: configmsmqerrorwithconfig

### Broker transports

This section applies to the following transports:

 * SQL Server
 * RabbitMQ
 * Azure Storage Queues
 * Azure Service Bus

Broker based transports have no need for special remote-queue configuration. Example configurations can be seen [here](nservicebus/recoverability/configure-error-handling).