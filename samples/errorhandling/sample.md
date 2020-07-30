---
title: Automatic Retries
summary: Shows immediate and delayed retries when a handler throws an exception.
reviewed: 2020-07-30
component: Core
related:
- nservicebus/recoverability
---



include: recoverability-rename

Run the sample **without debugging**.

The message handler in both endpoints is set to throw an exception, causing the handled message to end up in the error queue. The portable Particular Service Platform will list the messages arriving in the error queue.

include: platformlauncher-windows-required

snippet: handler

The "With Delayed Retries" endpoint uses the standard Delayed Retries settings.

The "Disable Delayed Retries" endpoint disables Delayed Retries with the following:

snippet: Disable

## The output

WARNING: This sample uses `Console.Writeline` instead of standard logging only for brevity and should not be used in production code.

### Without Delayed Retries

In this endpoint, the message is retried successively without any delay and then, after the final failure, it is forwarded to the configured error queue.

```
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

In this endpoint, the message is tried successively first and then delayed for the configured amount of time and then retried again. After the final configured retry, the message is moved to the error queue. The sample displays the retry number for clarity.

```
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
