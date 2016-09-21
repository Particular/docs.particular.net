---
title: Managing Errors and Retries
summary: View the details of Failed Messages with ServiceInsight and the Retry them
component: ServiceInsight
reviewed: 2016-06-02
tags:
- ServiceInsight
---

When message processing fails NServiceBus will [automatically retry it](/nservicebus/recoverability/). If a message continues to fail it will be forwarded to the error queue and become visible within ServiceInsight.

The views in ServiceInsight show information about message processing failure with the message. No manual correlation of log files or accessing of remote servers is necessary to research the reasons for an error.


## Status in the Message List

The status of an errant message is illustrated in the message window.

![An Error in the Message Window](images/overview-messagewindowerror.png 'width=500')


## The Flow Diagram

The flow diagram highlights errors in red and provides details with access to the stack trace.

![Error in the flow diagram](images/overview-flowdiagramwitherror.png 'width=500')


## The Sequence Diagram

The sequence diagram highlights handlers with errors in red.

![Error in the sequence diagram](images/overview-sequence-diagram-witherror.png 'width=500')


## Retrying a failed message

Once the underlying cause for message processing failure has been resolved it is possible to retry a failed message from within ServiceInsight. Find the message to be retried and click `Retry Message`.
