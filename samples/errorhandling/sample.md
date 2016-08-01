---
title: Automatic Retries
summary: With Delayed Retries, the message causing the exception is instantly retried via a retries queue instead of an error queue.
reviewed: 2016-03-21
component: Core
tags:
- Delayed Retries
- Error Handling
- Exceptions
- Retry
related:
- nservicebus/recoverability
---

include: recoverability-rename

Run the sample **without debugging**.

Both endpoints execute the same code.

snippet: handler

The "With Delayed Retries" endpoint uses the standard Delayed Retries settings.

The "Disable Delayed Retries" endpoint disables Delayed Retries with the following

snippet:Disable


## The output


### Without Delayed Retries

```no-highlight
ReplyToAddress: Samples.ErrorHandling.WithoutDelayedRetries MessageId:91cc7d3b-b763-4e01-9a3b-a42f0014f233
ReplyToAddress: Samples.ErrorHandling.WithoutDelayedRetries MessageId:91cc7d3b-b763-4e01-9a3b-a42f0014f233
ReplyToAddress: Samples.ErrorHandling.WithoutDelayedRetries MessageId:91cc7d3b-b763-4e01-9a3b-a42f0014f233
ReplyToAddress: Samples.ErrorHandling.WithoutDelayedRetries MessageId:91cc7d3b-b763-4e01-9a3b-a42f0014f233
ReplyToAddress: Samples.ErrorHandling.WithoutDelayedRetries MessageId:91cc7d3b-b763-4e01-9a3b-a42f0014f233
2015-01-29 01:16:18.480 ERROR NServiceBus.Faults.Forwarder.FaultManager Message with '91cc7d3b-b763-4e01-9a3b-a42f0014f33' ID has failed Immediate Retries and will be moved to the configured error queue.
```


### With Delayed Retries

```no-highlight
2015-01-29 01:13:57.517 WARN  NServiceBus.Faults.Forwarder.FaultManager Message with '24ea8afe-7610-41a0-b201-a42f00143fb4' ID has failed Immediate and will be handed over to Delayed Retries for retry attempt 2.
ReplyToAddress: Samples.ErrorHandling.WithDelayedRetries MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
This is retry number 2
ReplyToAddress: Samples.ErrorHandling.WithDelayedRetries MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
ReplyToAddress: Samples.ErrorHandling.WithDelayedRetries MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
ReplyToAddress: Samples.ErrorHandling.WithDelayedRetries MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
ReplyToAddress: Samples.ErrorHandling.WithDelayedRetries MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
2015-01-29 01:14:18.537 WARN  NServiceBus.Faults.Forwarder.FaultManager Message with '24ea8afe-7610-41a0-b201-a42f00143fb4' ID has failed Immediate Retries and will be handed over to Delayed Retries for retry attempt 3.
ReplyToAddress: Samples.ErrorHandling.WithDelayedRetries MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
This is retry number 3
ReplyToAddress: Samples.ErrorHandling.WithDelayedRetries MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
ReplyToAddress: Samples.ErrorHandling.WithDelayedRetries MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
ReplyToAddress: Samples.ErrorHandling.WithDelayedRetries MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
ReplyToAddress: Samples.ErrorHandling.WithDelayedRetries MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
2015-01-29 01:14:49.573 ERROR NServiceBus.Faults.Forwarder.FaultManager Delayed Retries has failed to resolve the issue with message 24ea8afe-7610-41a0-b201-a42f00143fb4 and will be forwarded to the error queue at error
```