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
 - Faults
redirects:
- nservicebus/configure-error-queue
related:
 - samples/faulttolerance
---

## Configure the error queue address

When message processing fails NServiceBus performs automatic [delivery retries](/nservicebus/recoverability). When that fails as well NServiceBus forwards failing message to a designated error queue.

WARNING: When running with [transport transactions disabled](/nservicebus/transports/transactions.md#transactions-unreliable-transactions-disabled) NServiceBus will perform a best-effort error message forwarding, i.e. if moving to the error queue fails, the message will be lost.

WARNING: When running with [transport transactions disabled](/nservicebus/transports/transactions.md#transactions-unreliable-transactions-disabled). Both [Immediate Retries](/nservicebus/recoverability/#immediate-retries) and [Delayed Retries](/nservicebus/recoverability/#delayed-retries) will be automatically disabled when transactions are turned off.

Error queue address can be configured in several ways.

partial:ErrorWithCode


### Using a IConfigurationProvider

snippet:ErrorQueueConfigurationProvider


### Using a ConfigurationSource

snippet: ErrorQueueConfigurationSource

Then at configuration time:

snippet:UseCustomConfigurationSourceForErrorQueueConfig


### Using App.Config

snippet:configureErrorQueueViaXml

partial:errorheader


## Error queue monitoring

Administrators should monitor the error queue, in order to detect when problems occur. The message in the error queue contains information regarding its source queue and machine. Having that information an administrator can investigate the specific node and solve the problem, e.g. bring up a database that went down.

Monitoring and handling of failed messages with [ServicePulse](/servicepulse/) provides access to full exception details (including stack-trace). [ServiceInsight](/serviceinsight/) enables additional, advanced debugging with providing full message processing context. Both of them provide a `retry` functionality to send the message back to the endpoint for re-processing. For more details, see [Introduction to Failed Messages Monitoring in ServicePulse](/servicepulse/intro-failed-messages.md).

If either ServicePulse or ServiceInsight is not available in the environment, the `retry` operation can be performed using the native management tools of the selected transport:

 * [Msmq Scripting](/nservicebus/msmq/operations-scripting.md)
 * [RabbitMq Scripting](/nservicebus/rabbitmq/operations-scripting.md)
 * [SqlServer Scripting](/nservicebus/sqlserver/operations-scripting.md)

### ReturnToSourceQueue.exe

The MSMQ command line tool `ReturnToSourceQueue` has been deprecated and moved to [ParticularLabs/MsmqReturnToSourceQueue](https://github.com/ParticularLabs/MsmqReturnToSourceQueue/).