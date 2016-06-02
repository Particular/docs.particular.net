---
title: Managing Errors and Retries in ServiceInsight
summary: How to view the details of Failed Messages with ServiceInsight and the Retry them
reviewed: 2016-06-02
tags:
- ServiceInsight
---

When message processing fails NServiceBus will [automatically retry it](/nservicebus/errors/automatic-retries). If a message continues to fail it will be forwarded to the [configured error queue](http://docs.particular.net/nservicebus/errors/) and become visible within ServiceInsight.

The views in ServiceInsight show information about message processing failure with the message. No manual correlation of log files or accessing of remote servers is necessary to research the reasons for an error. 


## Status in the Message List

The status of an errant message is illustrated in the message window.

![An Error in the Message Window](images/overview-messagewindowerror.png)


## The Flow Diagram

The flow diagram highlights errors in red and provides details with access to the stack trace.

![Error in the flow diagram](images/overview-flowdiagramwitherror.png)


## The Sequence Diagram

The sequence diagram highlights handlers with errors in red.

![Error in the sequence diagram](images/overview-sequence-diagram-witherror.png)


## Retrying a failed message

Once the underlying cause for message processing failure has been resolved it is possible to rety a failed message from within ServiceInsight. Find the message to be retried and click `Retry Message`.