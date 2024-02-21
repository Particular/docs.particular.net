---
title: Managing Errors and Retries
summary: View the details of failed messages with ServiceInsight and retry them
component: ServiceInsight
reviewed: 2024-02-21
---

When a message fails during processing, NServiceBus will [automatically retry it](/nservicebus/recoverability/). If a message continues to fail, it will be forwarded to the error queue and become visible within ServiceInsight.

The views in ServiceInsight show information about message processing failure with the message. No manual correlation of log files or access to remote servers is necessary to research the reasons for an error.

## Status in the message list

The status of an errant message is illustrated in the message window. A message can have one of the statuses below.

![An error in the message window](images/overview-messagewindowerror.png 'width=500')

**Successfully processed message**: A message that was successfully processed. Requires [message auditing](/nservicebus/operations/auditing.md) to be enabled. ![Success icon](images/status-success-icon.png)

**Message failed one time**: A message moved to the error queue only once. ![Single error icon](images/status-error-icon.png)

**Message failed multiple times**: A message that has been retried at least once using tools like ServiceInsight or ServicePulse, and for which retries weren't able to process the message successfully. ![Multiple errors icon](images/status-multiple-error-icon.png)

**Retry requested for message**: A failed message for which a retry was requested from ServiceInsight or [ServicePulse](/servicepulse). If a message remains with this status, there is no further information about that message, neither failure nor audit for a successful processing. Once the message is re-processed, the status will change to resolved successfully, failed once, or failed multiple times. ![Retry icon](images/status-retry-icon.png)

**Resolved successfully**: The message was successfully processed after failure(s). ![Retry icon](images/status-resolved-successfully.png)

## Clock drift

If the status icon has an exclamation sign overlay (⚠), it is shown when the calculated [critical time](/monitoring/metrics/definitions.md#metrics-captured-critical-time) is *negative* usually caused by [clock drift](https://en.wikipedia.org/wiki/Clock_drift). Clock drift can be mitigated by frequently syncing against the same shared time source via NTP.

## The flow diagram

The flow diagram highlights errors in red and provides details with access to the stack trace.

![Error in the flow diagram](images/overview-flowdiagramwitherror.png 'width=500')

## The sequence diagram

The sequence diagram highlights handlers with errors in red.

![Error in the sequence diagram](images/overview-sequence-diagram-witherror.png 'width=500')


## Retrying a failed message

Once the underlying cause for message processing failure has been resolved, the failed message can be retried from ServiceInsight. Find the message to be retried and click `Retry Message`.
