---
title: Error handling
summary: Don't try to handle exceptions in your message handlers. Let NServiceBus do it for you.
tags:
- Exceptions
- Error Handling
- Automatic Retries
redirects:
 - nservicebus/how-do-i-handle-exceptions
related:
- nservicebus/operations/transactions-message-processing
---

NServiceBus has exception catching and handling logic of its own which surrounds all calls to user code. When an exception bubbles through to the NServiceBus infrastructure, it rolls back the transaction on a transactional endpoint, causing the message to be returned to the queue, and any messages that the user code tried to send or publish to be undone as well.


## Configure your error queue

When a message fails NServiceBus [automatically retries](/nservicebus/errors/automatic-retries.md) the message. On repeated failure NServiceBus forwards that message to a designated error queue. 

Error queue can be configured in several ways. 


### Using Code

You can configure the target error queue at configuration time.

snippet:ErrorWithCode


### Using a IConfigurationProvider 

snippet:ErrorQueueConfigurationProvider


### Using a ConfigurationSource

<!-- import ErrorQueueConfigurationSource-->

Then at configuration time:

snippet:UseCustomConfigurationSourceForErrorQueueConfig


### Using App.Config

snippet:configureErrorQueueViaXml

NOTE: In NServiceBus version 3.x the `ErrorQueue` settings can be set both via the new `MessageForwardingInCaseOfFaultConfig ` section and the old `MsmqTransportConfig` section.

For more details on `MsmqTransportConfig` [read this article](/nservicebus/msmq/transportconfig.md).


## Monitor your error queue

Administrators should monitor that error queue so that they can see when problems occur. The message in the error queue contains the source queue and machine so that the administrator can see what's wrong with that node and possibly correct the problem, for example, bringing up a database that went down.

Monitoring and handling of failed messages with [ServicePulse](/servicepulse) provides access to full exception details (including stack-trace, and through ServiceInsight it also enables advanced debugging with all message context. It also provides a "retry" option to send the message back to the endpoint for re-processing. For more details, see [Introduction to Failed Messages Monitoring in ServicePulse](/servicepulse/intro-failed-messages.md). 

If either ServicePulse or ServiceInsight is not available in your environment you can perform this operation using the native management tools of your selected transport of by code or powershell:

 * [Msmq Scripting](/nservicebus/msmq/operations-scripting.md)
 * [RabbitMq Scripting](/nservicebus/rabbitmq/operations-scripting.md)
 * [SqlServer Scripting](/nservicebus/sqlserver/operations-scripting.md)

### ReturnToSourceQueue.exe

The MSMQ command line tool ReturnToSourceQueue has been deprecated and moved to [ParticularLabs/MsmqReturnToSourceQueue](https://github.com/ParticularLabs/MsmqReturnToSourceQueue/).
