---
title: Automatic Retries
summary: The message which caused the exception during processing, is automatically retried for the configured number of times before being forwarded to the error queue.
tags:
- Second Level Retry
- Error Handling
- Exceptions
- Retry
reviewed: 2016-03-31
redirects:
 - nservicebus/second-level-retries
related:
- samples/faulttolerance
---

Sometimes processing of a message fails. This could be due to a transient problem like a deadlock in the database, in which case retrying the message a few times should solve the issue. If the problem is more protracted, like a third party web service going down or a database being unavailable, solving the issue would take longer. It is therefore useful to wait longer before retrying the message again.

NServiceBus offers two levels of retries:

- First Level Retry(FLR) is for transient errors, where quick successive retries solve the problem.
- Second Level Retry(SLR) is for errors that persist after FLR, where a small delay is needed between retries.

NOTE: When a message cannot be deserialized, it will bypass all retry mechanisms and the message will be moved directly to the error queue.


## First Level Retries

NServiceBus automatically retries the message when an exception is thrown during message processing for up to five times by default. This value can be configured through `app.config` or via code.

Note: The configured value describes the minimum number of times a message will be retried. Especially in environments with competing consumers on the same queue, there is an increased chance of retrying a failing message more times across the endpoints.


### Transport transaction requirements

The FLR mechanism is implemented by making the message available for consumption again at the top of the queue, so that the endpoint can process it again immediately. FLR cannot be used when transport transactions are disabled. For more information about transport transactions, refer to [transport transaction](/nservicebus/transports/transactions.md).

### Configuring FLR using app.config

In Version 3 this configuration was available via `MsmqTransportConfig`.

In Version 4 and above the configuration for this mechanism is implemented in the `TransportConfig` section.

snippet:configureFlrViaXml


### Configuring FLR through IProvideConfiguration

snippet:FlrProvideConfiguration


### Configuring FLR through ConfigurationSource

snippet:FLRConfigurationSource

snippet:FLRConfigurationSourceUsage


## Second Level Retries

SLR introduces another level of retry mechanism for messages that fail processing. SLR picks up the message and defers its delivery, by default first for 10 seconds, then 20, and lastly for 30 seconds, then returns it to the original worker queue.

For example, if there is a call to an web service in the handler, but the service goes down for five seconds just at that time. Without SLR, the message is retried instantly and sent to the error queue. With SLR, the message is instantly retried, deferred for 10 seconds, and then retried again. This way, when the Web Service is available the message is processed just fine.

NOTE: Retrying messages for extended periods of time would hide failures from operators, thus preventing them from taking manual action to honor their Service Level Agreements. To avoid this, NServiceBus will make sure that no message is retried for more than 24 hours before being sent the error queue.


### Transport transaction requirements

The SLR mechanism is implemented by rolling back the [transport transaction](/nservicebus/transports/transactions.md) and scheduling the message for [delayed-delivery](/nservicebus/messaging/delayed-delivery.md). Aborting the receive operation when transactions are turned off would result in a message loss. Therefore SLR cannot be used when transport transactions are disabled.


### Configuring SLR using app.config

To configure SLR, enable its configuration section:

snippet:SecondLevelRetiesAppConfig

 * `Enabled`: Turns the feature on and off. Default: true.
 * `TimeIncrease`: A time span after which the time between retries increases. Default: 10 seconds (`00:00:10`).
 * `NumberOfRetries`: Number of times SLR kicks in. Default: 3.


### Configuration SLR through IProvideConfiguration

snippet:SlrProvideConfiguration


### Configuring SLR through ConfigurationSource

snippet:SlrConfigurationSource

snippet:SLRConfigurationSourceUsage


### Disabling SLR through code

snippet:DisableSlrWithCode


### Custom Retry Policy

You can apply custom retry logic based on headers or timing in code.


#### Applying a custom policy

snippet:SecondLevelRetriesCustomPolicy


#### Error Headers Helper

A Custom Policy has access to the raw message including both the [retries handling headers](/nservicebus/messaging/headers.md#retries-handling-headers) and the [error forwarding headers](/nservicebus/messaging/headers.md#error-forwarding-headers). Any of these headers can be used to control the retries for a message. In the following examples the helper class will provide access to a subset of the headers.

snippet:ErrorsHeadersHelper


#### Simple Policy

The following retry policy that will retry a message 3 times with a 5 second interval.

snippet:SecondLevelRetriesCustomPolicyHandler


#### Exception based Policy

The following retry policy extends the previous policy with a custom handling logic for a specific exception.

snippet:SecondLevelRetriesCustomExceptionPolicyHandler


## Total number of possible retries

The total number of possible retries can be calculated with the following formula

    Total Attempts = (FLR:MaxRetries) * (SLR:NumberOfRetries + 1)

So for example given a variety of FLR and SLR here are the resultant possible attempts.

| FLR:MaxRetries | SLR:NumberOfRetries | Total possible attempts |
|----------------|---------------------|-------------------------|
| 1              | 1                   | 2                       |
| 1              | 2                   | 3                       |
| 1              | 3                   | 4                       |
| 2              | 1                   | 4                       |
| 3              | 1                   | 6                       |
| 2              | 2                   | 6                       |

NOTE: In Versions 6 and higher, the configuration of the FLR mechanism will have no effect on how many times a deferred message is dispatched when an exception is thrown. SLR will retry the message for the number of times specified in its configuration.


## Retry Logging

Given the following configuration:

 * FLR `MaxRetries`: 3
 * SLR `NumberOfRetries`: 2

and a Handler that both throws an exception and logs the current count of attempts, the output in the log will be:

snippet:RetryLogging

Note that in some cases a log entry contains the exception (`Exception included`) and in some cases it is omitted (`Exception omitted`)
