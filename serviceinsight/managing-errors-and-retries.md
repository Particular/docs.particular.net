---
title: Managing Errors and Retries
summary: View the details of failed messages with ServiceInsight and retry them
component: ServiceInsight
reviewed: 2018-06-07
---

When a message fails during processing, NServiceBus will [automatically retry it](/nservicebus/recoverability/). If a message continues to fail it will be forwarded to the error queue and become visible within ServiceInsight.

The views in ServiceInsight show information about message processing failure with the message. No manual correlation of log files or accessing of remote servers is necessary to research the reasons for an error.


## Status in the message list

The status of an errant message is illustrated in the message window.

![An error in the message window](images/overview-messagewindowerror.png 'width=500')


## The flow diagram

The flow diagram highlights errors in red and provides details with access to the stack trace.

![Error in the flow diagram](images/overview-flowdiagramwitherror.png 'width=500')


## The sequence diagram

The sequence diagram highlights handlers with errors in red.

![Error in the sequence diagram](images/overview-sequence-diagram-witherror.png 'width=500')


## Retrying a failed message

Once the underlying cause for message processing failure has been resolved, the failed message can be retried from within ServiceInsight. Find the message to be retried and click `Retry Message`.
