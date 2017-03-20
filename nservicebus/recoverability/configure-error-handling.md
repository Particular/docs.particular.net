---
title: Configure error handling
summary: Messages that failed a certain number of times will be forwarded to the error queue. This page shows how to configure error handling.
component: Core
reviewed: 2016-10-21
tags:
 - Error Handling
 - Exceptions
 - Retry
 - Recoverability
redirects:
- nservicebus/configure-error-queue
related:
 - samples/faulttolerance
---

## Configure the error queue address

When an endpoint fails to process a message successfully, NServiceBus [automatically retries](/nservicebus/recoverability/configure-immediate-retries.md) the message for the configured number of times. If the message could not be processed successfully even after the retried attempts, then NServiceBus forwards that message to the designated error queue.

WARNING: When running with [transport transactions disabled](/nservicebus/transports/transactions.md#transactions-unreliable-transactions-disabled) NServiceBus will perform a best-effort error message forwarding, i.e. if moving to the error queue fails, the message will be lost.

WARNING: When running with [transport transactions disabled](/nservicebus/transports/transactions.md#transactions-unreliable-transactions-disabled). Both [Immediate Retries](/nservicebus/recoverability/#immediate-retries) and [Delayed Retries](/nservicebus/recoverability/#delayed-retries) will be automatically disabled when transactions are turned off.

Error queue address can be configured in several ways.

partial: ErrorWithCode

include: configurationWarning

### Using a IConfigurationProvider

snippet: ErrorQueueConfigurationProvider


### Using a ConfigurationSource

snippet: ErrorQueueConfigurationSource

Then at configuration time:

snippet: UseCustomConfigurationSourceForErrorQueueConfig


### Using App.Config

snippet: configureErrorQueueViaXml

partial: errorheader


## Error queue monitoring

Administrators should monitor the error queue in order to detect when problems occur. The message in the error queue contains relevant information such as the endpoint that originally processed the message, exception details, etc. With that information an administrator can investigate the problem and solve it, e.g. bring up a database that went down.

Monitoring and handling of failed messages with [ServicePulse](/servicepulse/) provides access to full exception details (including stack-trace). [ServiceInsight](/serviceinsight/) offers advanced debugging capability providing additional information like exception details as well as visualizing the flow of messages. Both ServiceInsight and ServicePulse provide a `retry` functionality that sends the failed message from the error queue back to the endpoint for re-processing. For more details on how to retry a message using ServicePulse, see [Introduction to Failed Messages Monitoring in ServicePulse](/servicepulse/intro-failed-messages.md). To retry a message using ServiceInsight, see [Managing Errors and Retries in ServiceInsight](/serviceinsight/managing-errors-and-retries.md).

If either ServicePulse or ServiceInsight are not available in the environment, the `retry` operation can be performed using the native management tools appropriate for the selected transport:

 * [Msmq Scripting](/nservicebus/msmq/operations-scripting.md)
 * [RabbitMq Scripting](/nservicebus/rabbitmq/operations-scripting.md)
 * [SqlServer Scripting](/nservicebus/sqlserver/operations-scripting.md)


### ReturnToSourceQueue.exe

The MSMQ command line tool `ReturnToSourceQueue` has been deprecated and moved to [ParticularLabs/MsmqReturnToSourceQueue](https://github.com/ParticularLabs/MsmqReturnToSourceQueue/).
