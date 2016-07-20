---
title: Recoverability
summary: Don't try to handle exceptions in the message handlers. Let NServiceBus do it for you.
tags:
- Exceptions
- Error Handling
- Retry
- Recoverability
reviewed: 2016-03-31
redirects:
 - nservicebus/how-do-i-handle-exceptions
 - nservicebus/errors
related:
- nservicebus/operations/transactions-message-processing
---

NServiceBus has a built-in error handling capability called Recoverability. Recoverability enables to recover automatically or in exceptional scenarios manually from message failures. Recoverability wraps the message handling logic, including the user code with various layers of retrying logic. NServiceBus differentiates three types of retrying behaviors:

* Immediate Retry (previously known as First-Level-Retries)
* Delayed Retry (previously known as Second-Level-Retries)
* Faults

An oversimplified mental model for Recoverability could be though of a gigantic try / catch block surrounding the message handling infrastructure wrapped in a while loop like the following pseudo-code

```
Exception exception = null;
do
{
  try
  {
    messageHandling.Invoke();
    exception = null;
  } catch (Exception ex) {
    exception = ex;
  }
} while(exception != null)
```

Of course the reality is much more complex. Depending on the transports capabilities, the transactionality of the endpoint and the users customizations always tries to recover from message failures or at least makes sure messages that failed multiple times get moved to the configured error queue. For Recoverability to be able to work at is full capacity the following requirements should be met:

* Immediate Retry needs a transactional transport with at least [ReceiveOnly](/nservicebus/transports/transactions) transport transaction mode.
* Delayed Retry needs a transactional transport with at least [ReceiveOnly](/nservicebus/transports/transactions) transport transaction mode and the transport needs to support [delayed delivery](/nservicebus/messaging/delayed-delivery) natively or the timeout manager needs to be enabled.

When an exception bubbles through to the NServiceBus infrastructure, it rolls back the transaction on the transactional endpoint. The message is then returned to the queue, and any messages that the user code tried to send or publish won't be sent out.


## Configure the error queue

When a message fails NServiceBus [automatically retries](/nservicebus/errors/automatic-retries.md) the message. On repeated failure NServiceBus forwards that message to a designated error queue.

WARNING: When running with [transport transactions disabled](/nservicebus/transports/transactions.md#transactions-unreliable-transactions-disabled) NServiceBus will perform a best-effort error message forwarding, i.e. if moving to the error queue fails, the message will be lost.

WARNING: When running with [transport transactions disabled](/nservicebus/transports/transactions.md#transactions-unreliable-transactions-disabled). Both FLR and SLR will be silently disabled when transactions are turned off.

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
