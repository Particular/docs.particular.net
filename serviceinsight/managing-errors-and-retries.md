---
title: Managing Errors and Retries
summary: View the details of failed messages with ServiceInsight and retry them
component: ServiceInsight
reviewed: 2020-03-16
---

When a message fails during processing, NServiceBus will [automatically retry it](/nservicebus/recoverability/). If a message continues to fail it will be forwarded to the error queue and become visible within ServiceInsight.

The views in ServiceInsight show information about message processing failure with the message. No manual correlation of log files or accessing of remote servers is necessary to research the reasons for an error.

## Status in the message list

The status of an errant message is illustrated in the message window.

![An error in the message window](images/overview-messagewindowerror.png 'width=500')

A message can have one of the following status:

![Retry icon](images/status-retry-icon.png 'width=20') **Retried message**: A failed message, for which a retry was requested from ServiceInsight or ServicePulse. If a message remains with this status, there is no further information about that message, neither failure nor audit for a successful processing

![Multiple errors icon](images/status-multiple-error-icon.png 'width=20') **Message failed multiple times**: A message that has been retried at least once using tools like ServiceInsight or ServicePulse, and for which retries weren't able to process the message successfully.

![Single error icon](images/status-error-icon.png 'width=20') **Message failed one time**: A message that was moved to the error queue only one time.

![Success icon](images/status-success-icon.png 'width=20') **Successful processed message**: A message that was successfully processed. Requires [message auditing](/nservicebus/operations/auditing.md) to be enabled.

## The flow diagram

The flow diagram highlights errors in red and provides details with access to the stack trace.

![Error in the flow diagram](images/overview-flowdiagramwitherror.png 'width=500')


## The sequence diagram

The sequence diagram highlights handlers with errors in red.

![Error in the sequence diagram](images/overview-sequence-diagram-witherror.png 'width=500')


## Retrying a failed message

Once the underlying cause for message processing failure has been resolved, the failed message can be retried from ServiceInsight. Find the message to be retried and click `Retry Message`.
