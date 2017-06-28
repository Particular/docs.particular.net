---
title: Configuring MSMQ for use with ServiceControl
summary: How to configure MSMQ to feed data into ServiceControl
component: MSMQTransport
reviewed: 2017-06-28
versions: '[5,)'
related:
 - nservicebus/recoverability/configure-error-handling
---

ServiceControl reads messages from the input queue specified in the [ServiceControl instance transport configuration](creating-config-file#transport). Monitored endpoints send appropriate messages, e.g. error or heartbeats, to the configured queues. ServiceControl ingests those messages in order to extract required information and saves messages in internal storage. 

Depending on the selected transport and the deployment scenario, the production endpoints might require additional configuration for ServiceControl. Currently only MSMQ transport requires it.

The following transports don't need any additional configuration for remote queues:

 * SQL Server
 * RabbitMQ
 * Azure Storage Queues
 * Azure Service Bus

Sample configurations for those transports can be seen [here](nservicebus/recoverability/configure-error-handling).


## Remote MSMQ queues

If ServiceControl is installed on a different machine than endpoints using MSMQ Transport, then endpoints must be configured to send error and audit messages to a remote queue:

snippet: ConfigMsmqErrorWithCode

snippet: ErrorQueueRemoteMachineConfigurationProvider

snippet: ErrorQueueRemoteMachineConfigurationSource

snippet: UseCustomConfigurationSourceForErrorQueueRemoateMachineConfig