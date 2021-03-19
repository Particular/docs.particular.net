---
title: Recoverability
summary: Explains how exceptions are handled, and actions retried, during message processing
component: Core
isLearningPath: true
reviewed: 2020-07-23
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

The reality is more complex. Depending on the transports capabilities, the transactionality mode of the endpoint and user customizations recoverability tries to recover from message failures. For example on a transactional endpoint it will roll back receive transaction when an exception bubbles through to the NServiceBus infrastructure. The message is then returned to the input queue, and any messages that the user code tried to send or publish won't be sent out. The very least that recoverability will ensure is that messages which failed multiple times get moved to the configured error queue. The part of recoverability which is responsible to move failed messages to the error queue is called fault handling.

NOTE: When a message cannot be deserialized all retry mechanisms will be bypassed and the message will be moved directly to the error queue.


## Immediate retries

By default up to five immediate retries are performed if the message processing results in exception being thrown. This value [can be configured](/nservicebus/recoverability/configure-immediate-retries.md).

Note: The configured value describes the minimum number of times a message will be retried if its processing consistently fails. Especially in environments with competing consumers on the same queue, there is an increased chance of retrying a failing message more times across the endpoints.

Note: The actual number of immediate retries performed is done on a best-effort basis. Immediate retries might be performed more than configured number of times (even if configured to be off) depending on the environment (e.g. when there are competing consumers). To ensure messages are processed once at most, the endpoint must be configured to use [transaction mode](/transports/transactions.md) `None`.


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


## Fault handling

When messages continuously failed during the immediate and delayed retries mechanisms they will be moved to the [error queue](/nservicebus/recoverability/configure-error-handling.md).


### Transport transaction requirements

Fault handling doesn't require that the transport transaction is rolled back. A copy of the currently handled message is sent to the configured error queue and the current transaction will be marked as successfully processed. Therefore fault handling works with all supported [transport transaction modes](/transports/transactions.md).


## Recoverability policy

partial: recoverabilitypolicy

partial: unrecoverableexceptions

## Total number of possible retries

partial: retrycount


## Retry logging

Given the following configuration:

 * Immediate retries `NumberOfRetries`: 3
 * Delayed retries `NumberOfRetries`: 2
 * A handler that both throws an exception and logs the current count of attempts

The output in the log will be:

snippet: RetryLogging

partial: exceptionincluded


## Recoverability memory consumption

MSMQ and SQL Server transport need to cache exceptions in memory for retries. Therefore, exceptions with a large memory footprint can cause high memory usage of the NServiceBus process. NServiceBus can cache up to 1,000 exceptions, capping the potential memory consumption to 1,000 x `<exception size>`. Refer to [this guide](/nservicebus/recoverability/lru-memory-consumption.md) to resolve problems due to excessive memory consumption.
