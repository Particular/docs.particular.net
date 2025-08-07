---
title: Automatic Retries
summary: Shows immediate and delayed retries when a handler throws an exception.
reviewed: 2025-08-04
component: Core
related:
- nservicebus/recoverability
---

This sample shows the different ways that NServiceBus [recoverability features](/nservicebus/recoverability/) retry messages that have failed.

## Running the sample

Run the sample **without debugging**.

The message handler in both endpoints (`WithoutDelayedRetries` and `WithDelayedRetries`) is set to throw an exception, causing the handled message to end up in the error queue. The portable Particular Service Platform will list the messages arriving in the error queue.

snippet: handler

The `WithDelayedRetries` endpoint uses the standard Delayed Retries settings.

The `WithoutDelayedRetries` endpoint disables Delayed Retries with the following:

snippet: Disable

## The output

> [!WARNING]
> This sample uses `Console.WriteLine` instead of standard logging only for brevity and should not be used in production code.

### Without Delayed Retries

In this endpoint, the message is retried successively without any delay and then, after the final failure, it is forwarded to the configured error queue.

```bash
Handling MyMessage with MessageId:b5d0ea24-63c7-4729-8fd3-a6dc0161a7f8
Handling MyMessage with MessageId:b5d0ea24-63c7-4729-8fd3-a6dc0161a7f8
Handling MyMessage with MessageId:b5d0ea24-63c7-4729-8fd3-a6dc0161a7f8
Handling MyMessage with MessageId:b5d0ea24-63c7-4729-8fd3-a6dc0161a7f8
Handling MyMessage with MessageId:b5d0ea24-63c7-4729-8fd3-a6dc0161a7f8
Handling MyMessage with MessageId:b5d0ea24-63c7-4729-8fd3-a6dc0161a7f8
2016-12-14 13:27:41.232 ERROR NServiceBus.RecoverabilityExecutor Moving message 'b5d0ea24-63c7-4729-8fd3-a6dc0161a7f8' to the error queue 'error' because processing failed due to an exception:
System.Exception: An exception occurred in the handler.
```

### With Delayed Retries

In this endpoint, the message is first [retried immediately](/nservicebus/recoverability/#immediate-retries), then retried again after a [configured delay](/nservicebus/recoverability/configure-delayed-retries.md). After the final configured retry, the message is moved to the error queue. The sample displays the retry number for clarity.

```bash
This is retry number 1
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
2016-12-14 13:33:18.790 WARN  NServiceBus.RecoverabilityExecutor Delayed Retry will reschedule message '05b97154-04b9-405a-92d7-a6dc0163273f' after a delay of 00:00:20 because of an exception:
System.Exception: An exception occurred in the handler.
. . .
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
This is retry number 2
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
2016-12-14 13:33:40.886 WARN  NServiceBus.RecoverabilityExecutor Delayed Retry will reschedule message '05b97154-04b9-405a-92d7-a6dc0163273f' after a delay of 00:00:30 because of an exception:
System.Exception: An exception occurred in the handler.
. . .
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
This is retry number 3
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
Handling MyMessage with MessageId:05b97154-04b9-405a-92d7-a6dc0163273f
2016-12-14 13:34:15.750 ERROR NServiceBus.RecoverabilityExecutor Moving message '05b97154-04b9-405a-92d7-a6dc0163273f' to the error queue 'error' because processing failed due to an exception:
System.Exception: An exception occurred in the handler.
. . .
```
