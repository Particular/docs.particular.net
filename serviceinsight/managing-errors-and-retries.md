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

The are the following statuses displayed (described from the top):
 - Retried message is a failed message, which retry was requested from ServiceInsight. If a message stays in this status it means that there is no further information about that message, neither failure nor audit for a successful processing
 - Message failed multiple times is a message that was at least once retried using tools like ServiceInsight or ServicePulse and it again returned back as a failure
 - Message failed one time, is a message that for the first time got to error queue and was processed by ServiceControl
 - Successful processed message

## The flow diagram

The flow diagram highlights errors in red and provides details with access to the stack trace.

![Error in the flow diagram](images/overview-flowdiagramwitherror.png 'width=500')


## The sequence diagram

The sequence diagram highlights handlers with errors in red.

![Error in the sequence diagram](images/overview-sequence-diagram-witherror.png 'width=500')


## Retrying a failed message

Once the underlying cause for message processing failure has been resolved, the failed message can be retried from ServiceInsight. Find the message to be retried and click `Retry Message`.
