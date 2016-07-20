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
 - nservicebus/automatic-retries
related:
- nservicebus/operations/transactions-message-processing
---
Sometimes processing of a message fails. This could be due to a transient problem like a deadlock in the database, in which case retrying the message a few times should solve the issue. If the problem is more protracted, like a third party web service going down or a database being unavailable, solving the issue would take longer. It is therefore useful to wait longer before retrying the message again.

NServiceBus has a built-in error handling capability called Recoverability. Recoverability enables to recover automatically or in exceptional scenarios manually from message failures. Recoverability wraps the message handling logic, including the user code with various layers of retrying logic. NServiceBus differentiates two types of retrying behaviors:

* Immediate Retries (previously known as First-Level-Retries)
* Delayed Retries (previously known as Second-Level-Retries)

An oversimplified mental model for Recoverability could be thought of a try / catch block surrounding the message handling infrastructure wrapped in a while loop like the following pseudo-code

```cs
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

The reality is much more complex. Depending on the transports capabilities, the transactionality of the endpoint and the users customizations always tries to recover from message failures. In example when an exception bubbles through to the NServiceBus infrastructure, it rolls back the transaction on a transactional endpoint. The message is then returned to the queue, and any messages that the user code tried to send or publish won't be sent out. The very least that Recoverability will ensure is that messages which failed multiple times get moved to the configured error queue. The part of Recoverability which is responsible to move failed messages to the error queue is called fault handling.

NOTE: When a message cannot be deserialized, it will bypass all retry mechanisms and the message will be moved directly to the error queue.

## Immediate Retries

NServiceBus automatically and immediately retries the message when an exception is thrown during message processing for up to five times by default. This value can be configured through `app.config` or via code. For more information about how to configure immediate retries, refer to [configure immediate retries](/nservicebus/recoverability/configure-immediate-retries.md).

Note: The configured value describes the minimum number of times a message will be retried. Especially in environments with competing consumers on the same queue, there is an increased chance of retrying a failing message more times across the endpoints.

### Transport transaction requirements

The Immediate Retry mechanism is implemented by making the message available for consumption again at the top of the queue, so that the endpoint can process it again immediately. Immediate Retries cannot be used when transport transactions are disabled. For more information about transport transactions, refer to [transport transaction](/nservicebus/transports/transactions.md).

## Delayed Retries

Delayed Retries introduces another level of retry mechanism for messages that fail processing. Delayed Retries picks up the message and defers its delivery, by default first for 10 seconds, then 20, and lastly for 30 seconds, then returns it to the original worker queue.

For example, if there is a call to an web service in the handler, but the service goes down for five seconds just at that time. Without Delayed Retries, the message is retried instantly and sent to the error queue. With Delayed Retries, the message is instantly retried, deferred for 10 seconds, and then retried again. This way, when the Web Service is available the message is processed just fine.

For more information about how to configure delayed retries, refer to [configure delayed retries](/nservicebus/recoverability/configure-delayed-retries.md).

NOTE: Retrying messages for extended periods of time would hide failures from operators, thus preventing them from taking manual action to honor their Service Level Agreements. To avoid this, NServiceBus will make sure that no message is retried for more than 24 hours before being sent the error queue.

### Transport transaction requirements

The Delayed Retries mechanism is implemented by rolling back the [transport transaction](/nservicebus/transports/transactions.md) and scheduling the message for [delayed-delivery](/nservicebus/messaging/delayed-delivery.md). Aborting the receive operation when transactions are turned off would result in a message loss. Therefore Delayed Retries cannot be used when transport transactions are disabled and delayed-delivery is not supported.

## Fault handling

When messages failed during the immediate retries and delayed retries mechanism they will be moved to the error queue. Fault handling requires a configured error queue. For more information how to configure the error queue refer to [configure error queue](/nservicebus/recoverability/configure-error-queue.md).

### Transport transaction requirements

Fault handling doesn't require to roll back the transport transaction. A copy of the currently handled message is sent to the configured error queue and the current transaction will be marked as successfully processed. Therefore fault handling works with all supported [transport transaction modes](/nservicebus/transports/transactions.md).

## Total number of possible retries

The total number of possible retries can be calculated with the following formula

    Total Attempts = (ImmediateRetries:MaxRetries) * (DelayedRetries:NumberOfRetries + 1)

So for example given a variety of FLR and SLR here are the resultant possible attempts.

| ImmediateRetries:MaxRetries | DelayedRetries:NumberOfRetries | Total possible attempts |
|-----------------------------|--------------------------------|-------------------------|
| 1                           | 1                              | 2                       |
| 1                           | 2                              | 3                       |
| 1                           | 3                              | 4                       |
| 2                           | 1                              | 4                       |
| 3                           | 1                              | 6                       |
| 2                           | 2                              | 6                       |

NOTE: In Versions 6 and higher, the configuration of the Immediate Retries mechanism will have no effect on how many times a deferred message is dispatched when an exception is thrown. Delayed Retries will retry the message for the number of times specified in its configuration.

## Retry Logging

Given the following configuration:

 * Immediate Retries `MaxRetries`: 3
 * Delayed Retries `NumberOfRetries`: 2

and a Handler that both throws an exception and logs the current count of attempts, the output in the log will be:

snippet:RetryLogging

Note that in some cases a log entry contains the exception (`Exception included`) and in some cases it is omitted (`Exception omitted`).
