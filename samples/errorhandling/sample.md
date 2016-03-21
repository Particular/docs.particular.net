---
title: Automatic Retries
summary: With SLR, the message causing the exception is instantly retried via a retries queue instead of an error queue.
reviewed: 2016-03-21
tags:
- Second Level Retry
- Error Handling
- Exceptions
- Retry
related:
- nservicebus/errors
---

Run the sample **without debugging**.

Both endpoints execute the same code.

snippet: handler

The "With SLR" endpoint uses the standard SLR settings.

The "Disable SLR" endpoint disables SLR with the following

snippet:DisableSLR


## The output


### Without SLR

```
ReplyToAddress: Samples.ErrorHandling.WithoutSLR MessageId:91cc7d3b-b763-4e01-9a3b-a42f0014f233
ReplyToAddress: Samples.ErrorHandling.WithoutSLR MessageId:91cc7d3b-b763-4e01-9a3b-a42f0014f233
ReplyToAddress: Samples.ErrorHandling.WithoutSLR MessageId:91cc7d3b-b763-4e01-9a3b-a42f0014f233
ReplyToAddress: Samples.ErrorHandling.WithoutSLR MessageId:91cc7d3b-b763-4e01-9a3b-a42f0014f233
ReplyToAddress: Samples.ErrorHandling.WithoutSLR MessageId:91cc7d3b-b763-4e01-9a3b-a42f0014f233
2015-01-29 01:16:18.480 ERROR NServiceBus.Faults.Forwarder.FaultManager Message with '91cc7d3b-b763-4e01-9a3b-a42f0014f33' ID has failed FLR and will be moved to the configured error queue.
```


### With SLR

```
2015-01-29 01:13:57.517 WARN  NServiceBus.Faults.Forwarder.FaultManager Message with '24ea8afe-7610-41a0-b201-a42f00143fb4' ID has failed FLR and will be handed over to SLR for retry attempt 2.
ReplyToAddress: Samples.ErrorHandling.WithSLR MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
This is second level retry number 2
ReplyToAddress: Samples.ErrorHandling.WithSLR MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
ReplyToAddress: Samples.ErrorHandling.WithSLR MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
ReplyToAddress: Samples.ErrorHandling.WithSLR MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
ReplyToAddress: Samples.ErrorHandling.WithSLR MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
2015-01-29 01:14:18.537 WARN  NServiceBus.Faults.Forwarder.FaultManager Message with '24ea8afe-7610-41a0-b201-a42f00143fb4' ID has failed FLR and will be handed over to SLR for retry attempt 3.
ReplyToAddress: Samples.ErrorHandling.WithSLR MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
This is second level retry number 3
ReplyToAddress: Samples.ErrorHandling.WithSLR MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
ReplyToAddress: Samples.ErrorHandling.WithSLR MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
ReplyToAddress: Samples.ErrorHandling.WithSLR MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
ReplyToAddress: Samples.ErrorHandling.WithSLR MessageId:24ea8afe-7610-41a0-b201-a42f00143fb4
2015-01-29 01:14:49.573 ERROR NServiceBus.Faults.Forwarder.FaultManager SLR has failed to resolve the issue with message 24ea8afe-7610-41a0-b201-a42f00143fb4 and will be forwarded to the error queue at error
```