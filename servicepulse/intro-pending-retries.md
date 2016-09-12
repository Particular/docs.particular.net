---
title: Pending Retries Message Management
summary: Describes how ServicePulse detects and monitors failed messages in the pending state, and allows retrying, or archiving them.
tags:
 - ServicePulse
reviewed: 2016-09-12
---


In Versions 1.6.6 and above, ServicePulse includes an additional screen to view and manage failed messages that have been requested to be retried but have not completed yet.

Pending retries messages can be found by going to the pending retries screen.

![Pending Retries Tab](images/pending-retries.png 'width=500')


### Pending Retries Messages

The Pending Retries Messages screen shows failed messages which have been retried, but the status of that retry is pending. The status of retried failures is updated when either the message is processed again as either an Audited message (success) or as a failed message.

Failed messages that are retried may stay in the pending state for the following reasons:

 * The retrying endpoint is not working (e.g. crashed or is scaled-out) and the retried messages is waiting in the queue and has not yet been processed.
 * The retrying endpoint does not have auditing enabled but has successfully processed the retried message.

The messages displayed in this screen can be filtered based on the time period by selecting one of the options, such as messages in the last 2 hours, messages in the previous day or week. The default option is set to display all of the pending messages.

![Period Filter](images/pending-retries-period-selection.png 'width=500')

Results can also be filtered by queue name using the search functionality:

![Queue Filter](images/pending-retries-filter-queues.png 'width=500')

The information about the message such as Failure timestamp, endpoint, stack trace of the error, etc.,  is displayed in the same manner as it on the [Failed Messages](intro-failed-messages.md) page providing additional information as follows:
 
 * **Redirect** Information if redirect is created for this queue.


### Message Retry

WARNING: Failed messages that are currently in the pending status can be retried again, however this feature should be used with care. Retrying pending messages can cause the endpoint to reprocess the same message multiple times. Person should ensure that this message was not processed or was processed incorrectly before using retry feature.

To retry a pending retry message, select the failed message(s) in the list and click the "Retry Selected" button.

Alternately a queue can be selected and the "Retry All" option can be used to retry all the messages targeted for the queue.

Message retry will use [message redirects](redirect.md) if the original endpoint has been redirected in ServicePulse.

WARNING: A pending retry message that is sent for retry will remain in the pending retry list until it is resolved or fails again.


### Mark as complete

When the audit feature is disabled in the endpoint that processes the failed message, the entry will remain in the pending state indefinitely even after the message has been successfully reprocessed by that endpoint. In this scenario, use the `Mark as complete` feature to manually mark the failed message as resolved. Once the message is marked as resolved, it will no longer appear in the pending retries message list.
