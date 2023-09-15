---
title: Recoverability
summary: Explains how exceptions are handled, and actions retried, during message processing
component: Core
isLearningPath: true
reviewed: 2023-09-15
redirects:
 - nservicebus/how-do-i-handle-exceptions
 - nservicebus/errors
 - nservicebus/automatic-retries
 - nservicebus/errors/automatic-retries
related:
 - nservicebus/operations/transactions-message-processing
---

Sometimes processing of a message fails. This could be due to a transient problem like a deadlock in the database, in which case retrying the message a few times should solve the issue. If the problem is more protracted, like a third party web service going down or a database being unavailable, solving the issue would take longer. It is therefore useful to wait longer before retrying the message again.

Recoverability is the built-in error handling capability. Recoverability enables to recover automatically, or in exceptional scenarios manually, from message failures. Recoverability wraps the message handling logic, including the user code with various layers of retrying logic. NServiceBus differentiates two types of retrying behaviors:

 * Immediate retries (previously known as First-Level-Retries)
 * Delayed retries (previously known as Second-Level-Retries)

An oversimplified mental model for Recoverability could be thought of a try / catch block surrounding the message handling infrastructure wrapped in a for loop:

snippet: Recoverability-pseudo-code

The reality is more complex, depending on the transport's capabilities, the transaction mode of the endpoint, and user customizations. For example, on a transactional endpoint it will roll back the receive transaction when an exception bubbles through to the NServiceBus infrastructure. The message is then returned to the input queue, and any messages that the user code tried to send or publish won't be sent out. The very least that recoverability will ensure is that messages which failed multiple times get moved to the configured error queue. The part of recoverability which is responsible to move failed messages to the error queue is called fault handling.

To prevent sending all incoming messages to the error queue during a major system outage (e.g. when a database or a third-party service is down), the recoverability mechanism allows enabling [automatic rate-limiting](#automatic-rate-limiting). When enabled, NServiceBus detects the outage after a configured number of consecutive failures and automatically switches to rate-limiting mode. In this mode, only one message is attempted to probe if the problem persists. Once a message can be processed correctly, the system automatically switches to regular mode.

When a message cannot be deserialized all retry mechanisms will be bypassed and the message will be moved directly to the error queue.

## Immediate retries

By default up to five immediate retries are performed if the message processing results in exception being thrown. The  [number of immediate retries can be configured](/nservicebus/recoverability/configure-immediate-retries.md).

The configured value describes the minimum number of times a message will be retried if its processing consistently fails. Especially in environments with competing consumers on the same queue, there is an increased chance of retrying a failing message more times across different endpoint instances.


### Transport transaction requirements

The immediate retry mechanism is implemented by making the message available for consumption again, so that the endpoint can process it again without any delay. Immediate retries cannot be used when [transport transactions](/transports/transactions.md) are disabled.


## Delayed retries

Delayed retries introduces another level of retry mechanism for messages that fail processing. Delayed retries schedules message delivery to the endpoint's input queue with increasing delay, by default first with 10 seconds delay, then 20, and lastly with 30 seconds delay. In each cycle, a full round of immediate retries will occur based on the configuration of the immediate retry policy. See [Total number of possible retries](#total-number-of-possible-retries) later in this document for more information on how immediate and delayed retries work together.

Delayed retries might be useful when dealing with unreliable third-party resources - for example, if there is a call to a web service in the handler, but the service goes down for a couple of seconds once in a while. Without delayed retries, the message is retried instantly and sent to the error queue. With delayed retries, the message is instantly retried, deferred for 10 seconds, and then retried again. This way, when the web service is available the message is processed just fine.

For more information about how to configure delayed retries, refer to [configure delayed retries](configure-delayed-retries.md).

For more information how delayed retries work internally, refer to the [Delayed delivery - how it works](/nservicebus/messaging/delayed-delivery.md#how-it-works) section.

NOTE: Retrying messages for extended periods of time would hide failures from operators, thus preventing them from taking manual action to honor their Service Level Agreements. To avoid this, NServiceBus will make sure that the time between two consecutive delayed retries is no more than 24 hours before being sent the error queue.


### Transport transaction requirements

The delayed retries mechanism is implemented by rolling back the [transport transaction](/transports/transactions.md) and scheduling the message for [delayed-delivery](/nservicebus/messaging/delayed-delivery.md). Aborting the receive operation when transactions are turned off would result in a message loss. Therefore delayed retries cannot be used when transport transactions are disabled and delayed-delivery is not supported.

## Automatic rate limiting

The automatic rate limiting in response to consecutive message processing failures is designed to act as an [automatic circuit breaker](https://en.wikipedia.org/wiki/Circuit_breaker) preventing a large number of messages from being redirected to the `error` queue in the case of an outage of a resource required for processing of all messages (e.g. a database or a third-party service).

The following code enables the detection of consecutive failures.

snippet: configure-consecutive-failures

When the endpoint detects a configured number of consecutive failures, it reacts by switching to a processing mode in which one message is attempted at a time. If processing fails, the endpoint waits for configured time and attempts to process the next message. The endpoint continues running in this mode until at least one message is processed successfully.

### Considerations when configuring automatic rate limiting

1. The number of consecutive failures must be big enough so that it doesn't trigger rate-limiting when only a few failed messages are processed by the endpoint.
2. Endpoints that process many different message types may not be a good candidates for this feature. When rate limiting is active, it affects the entire endpoint. Endpoints that are rate limited due to a failure for one message type will slow down processing of all message types handled by the endpoint.

## Fault handling

When messages continuously failed during the immediate and delayed retries mechanisms they will be moved to the [error queue](/nservicebus/recoverability/configure-error-handling.md).


### Transport transaction requirements

Fault handling doesn't require that the transport transaction is rolled back. A copy of the currently handled message is sent to the configured error queue and the current transaction will be marked as successfully processed. Therefore fault handling works with all supported [transport transaction modes](/transports/transactions.md).


## Recoverability policy

It is possible to take full control over the whole Recoverability process using a [custom recoverability policy](/nservicebus/recoverability/custom-recoverability-policy.md).

partial: unrecoverableexceptions

## Total number of possible retries


The total number of possible retries can be calculated with the following formula

```txt
Attempts = (ImmediateRetries:NumberOfRetries + 1) * (DelayedRetries:NumberOfRetries + 1)
```

Given a variety of immediate and delayed configuration values here are the resultant possible attempts.

| ImmediateRetries | DelayedRetries | Total possible attempts |
|------------------|----------------|-------------------------|
| 0                | 0              | 1                       |
| 1                | 0              | 2                       |
| 2                | 0              | 3                       |
| 3                | 0              | 4                       |
| 0                | 1              | 2                       |
| 1                | 1              | 4                       |
| 2                | 1              | 6                       |
| 3                | 1              | 8                       |
| 1                | 2              | 6                       |
| 2                | 2              | 9                       |
| 1                | 3              | 8                       |
| **5**            | **3**          | **24  (default)**       |

### Scale-out multiplier

NOTE: Retry behavior can be interpreted as if retries result in duplicates when scaled-out. Retry behavior can result in excessive processing attempts but no duplicate messages are created. Ensure that logging uses unique identifiers for each endpoint instance.

If an endpoint is scaled-out the number of processing attempts increase if instances are retrieving messages from the same queue and the transport does not have a native delivery counter.

Affected transports:

- Azure Storage Queues
- SQL Server
- RabbitMQ
- Amazon SQS
- MSMQ (only if running multiple instance on the same machine)

Unaffected transports:

- Azure Service Bus
- Azure Service Bus Legacy

Azure Service Bus transports use a native delivery counter which is incremented after any endpoint instance fetches a message from a (shared) queue. The native delivery counter guarantees that the retry number is the same regardless if the endpoint is scaled out.


The number of instances acts as a multiplier for the maximum number of attempts.

```txt
Minimum Attempts = (ImmediateRetries:NumberOfRetries + 1) * (DelayedRetries:NumberOfRetries + 1)
Maximum Attempts = MinimumAttempts * NumberOfInstances
```

Example:

When taking the default values for immediate and delayed retries (five and three, respectively) and 6 instances the total number of attempts will be a minimum of `(5+1)*(3+1)=24` attempts and a maximum of `24*6=144` attempts.

## Retry logging

### Event types

NServiceBus logs processing failures via the logger **NServiceBus.RecoverabilityExecutor** with various log levels and messages. 

| Action          | Log level     | Log message                                                                                    |
|-----------------|---------------|------------------------------------------------------------------------------------------------|
| Immediate retry | Informational | Immediate Retry is going to retry message 'XXX' because of an exception:                       |
| Delayed retry   | Warning       | Delayed Retry will reschedule message 'XXX' after a delay of HH:MM:SS because of an exception: |
| To error queue  | Error         | Moving message 'XXX' to the error queue 'error' because processing failed due to an exception: |

This allows to setup alerting in centralized logging solution. For example, to notify an administrator when logger **NServiceBus.RecoverabilityExecutor** logs an ERROR log entry.

### Output example

Given the following configuration:

 * Immediate retries `NumberOfRetries`: 3
 * Delayed retries `NumberOfRetries`: 2
 * A handler that both throws an exception and logs the current count of attempts

The output in the log will be:

snippet: RetryLogging


## Recoverability memory consumption

MSMQ and SQL Server transport need to cache exceptions in memory for retries. Therefore, exceptions with a large memory footprint can cause high memory usage of the NServiceBus process. NServiceBus can cache up to 1,000 exceptions, capping the potential memory consumption to 1,000 x `<exception size>`. Refer to [this guide](/nservicebus/recoverability/lru-memory-consumption.md) to resolve problems due to excessive memory consumption.
