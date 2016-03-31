---
title: Error handling
summary: Don't try to handle exceptions in the message handlers. Let NServiceBus do it for you.
tags:
- Exceptions
- Error Handling
- Retry
reviewed: 2016-03-31
redirects:
 - nservicebus/how-do-i-handle-exceptions
related:
- nservicebus/operations/transactions-message-processing
---

NServiceBus has a built-in exception catching and handling logic, which encompasses all calls to the user code. When an exception bubbles through to the NServiceBus infrastructure, it rolls back the transaction on the transactional endpoint. The message is then returned to the queue, and any messages that the user code tried to send or publish won't be sent out.


## Configure the error queue

When a message fails NServiceBus [automatically retries](/nservicebus/errors/automatic-retries.md) the message. On repeated failure NServiceBus forwards that message to a designated error queue.

WARNING: When running with [transport transactions disabled](/nservicebus/messaging/transactions.md#transactions-unreliable-transactions-disabled) NServiceBus will perform a best-effort error message forwarding, i.e. if moving to the error queue fails, the message will be lost.

Error queue can be configured in several ways.

### Using Code

snippet:ErrorWithCode


### Using a IConfigurationProvider

snippet:ErrorQueueConfigurationProvider


### Using a ConfigurationSource

snippet: ErrorQueueConfigurationSource

Then at configuration time:

snippet:UseCustomConfigurationSourceForErrorQueueConfig


### Using App.Config

snippet:configureErrorQueueViaXml

NOTE: In NServiceBus Version 3.x the `ErrorQueue` settings can be set both via the `MessageForwardingInCaseOfFaultConfig ` section and the `MsmqTransportConfig` section.

For more details on `MsmqTransportConfig` refer to the [MSMQ transport](/nservicebus/msmq/transportconfig.md) article.


## Error queue monitoring

Administrators should monitor the error queue, in order to detect when problems occur. The message in the error queue contains information regarding its source queue and machine. Having that information an administrator can investigate the specific node and solve the problem, e.g. bring up a database that went down.

Monitoring and handling of failed messages with [ServicePulse](/servicepulse) provides access to full exception details (including stack-trace). [ServiceInsight](/serviceinsight) additionally enables advanced debugging with a full message context. Both of them provide a `retry` functionality to send the message back to the endpoint for re-processing. For more details, see [Introduction to Failed Messages Monitoring in ServicePulse](/servicepulse/intro-failed-messages.md).

If either ServicePulse or ServiceInsight is not available in the environment, the `retry` operation can be performed using the native management tools of the selected transport:

 * [Msmq Scripting](/nservicebus/msmq/operations-scripting.md)
 * [RabbitMq Scripting](/nservicebus/rabbitmq/operations-scripting.md)
 * [SqlServer Scripting](/nservicebus/sqlserver/operations-scripting.md)

### ReturnToSourceQueue.exe

The MSMQ command line tool `ReturnToSourceQueue` has been deprecated and moved to [ParticularLabs/MsmqReturnToSourceQueue](https://github.com/ParticularLabs/MsmqReturnToSourceQueue/).
