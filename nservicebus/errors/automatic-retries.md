---
title: Automatic Retries
summary: With retries, the message causing the exception is instantly retried configured number of times before forwarding to the error queue.
tags:
- Second Level Retry
- Error Handling
- Exceptions
- Retry
redirects:
 - nservicebus/second-level-retries
related:
- samples/faulttolerance
---

Sometimes processing of a message can fail. This could be due to a transient problem like a deadlock in the database, in which case retrying the message a few times might overcome this problem. Or, if the problem is more protracted, like a third party web service going down or a database being unavailable, where it might be useful to wait a little longer before retrying the message again.

For situations like these, NServiceBus offers two levels of retries:

- First Level Retry(FLR) is for the transient errors where quick successive retries could solve the problem.
- Second Level Retry(SLR) is when a small delay is needed between retries.

NOTE: When a message cannot be deserialized, it will bypass all retry mechanisms both the FLR and SLR and the message will be moved directly to the error queue.


## First Level Retries

NServiceBus automatically retries the message when an exception is thrown during message processing up to five successive times by default. This value can be configured through `app.config` or code.

Note: The configured value describes the minimum number of times a message will be retried. Especially in environments with competing consumers on the same queue there is an increased chance of retrying a failing message more often across the endpoints.


### Configuring FLR using app.config

In Version 3 this configuration was available via `MsmqTransportConfig`.

In Version 4 and above the configuration for this mechanism is implemented in the `TransportConfig` section.

snippet:configureFlrViaXml


### Configuring FLR through IProvideConfiguration

snippet:FlrProvideConfiguration


### Configuring FLR through ConfigurationSource

snippet:FLRConfigurationSource

snippet:FLRConfigurationSourceUsage


NOTE: From Version 6, configuration of the FLR mechanism will have no effect on how many times a deferred message is dispatched when an exception is thrown. In such a case the `TimeoutManager` will attempt the dispatch five times.


## Second Level Retries

SLR introduces another level of retrying mechanism for messages that fail processing. When using SLR, the message that causes the exception is, as before, instantly retried, but instead of being sent to the error queue, it is sent to a retries queue.

SLR then picks up the message and defers it, by default first for 10 seconds, then 20, and lastly for 30 seconds, then returns it to the original worker queue.

For example, if there is a call to an web service in your handler, but the service goes down for five seconds just at that time. Without SLR, the message is retried instantly and sent to the error queue. With SLR, the message is instantly retried, deferred for 10 seconds, and then retried again. This way, when the Web Service is available the message is processed just fine.

NOTE: Retrying messages for extended periods of time would hide failures from operators preventing them from taking manual action to honor Service Level Agreements. To avoid this happening, due to miss-configured retry polices, NServiceBus will make sure that no message is retried for more than 24 hours before being sent the error queue.

SLR can be configured in several ways.


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

To completely disable SLR through code:

snippet:DisableSlrWithCode


### Custom Retry Policy

You can apply custom retry logic based on headers or timing in code.


#### Applying a custom policy

snippet:SecondLevelRetriesCustomPolicy


#### Error Headers Helper

A Custom Policy has access to the raw message including both the [retries handling headers](/nservicebus/messaging/headers.md#retries-handling-headers) and the [error forwarding headers](/nservicebus/messaging/headers.md#error-forwarding-headers). Any of these headers can be used to control the reties for a message. In the below examples the following helper class will provide access to a subset of the headers.

snippet:ErrorsHeadersHelper


#### Simple Policy

Here is a simple retry policy that will retry 3 times with a 5 second interval.

snippet:SecondLevelRetriesCustomPolicyHandler


#### Exception based Policy

Here is a policy that extends the above with custom handling for a specific exception.

snippet:SecondLevelRetriesCustomExceptionPolicyHandler


## Total number of possible attempts

The total number of possible attempts can be calculated with the below formula

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


## Retry Logging

Given the following configuration:

 * FLR `MaxRetries`: 3
 * SLR `NumberOfRetries`: 2

And a Handler that both throws and exception and logs the current count of attempts:

Then the resultant output in the log will be:

snippet:RetryLogging

Note that in some cases a log entry contains the exception (`Exception included`) and in some cases it is omitted (`Exception omitted`)
