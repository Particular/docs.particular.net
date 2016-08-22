---
title: Pending Retries Message Management in ServicePulse
summary: Describes how ServicePulse monitors archived failed messages, and allows unarchiving archived failed messages.
tags:
 - ServicePulse
reviewed: 2016-07-28
---


In Versions **1.6.6** and above ServicePulse includes a screen to view and manage failed messages that have been sent for retry but are pending final status.

Pending retries messages can be found by going to the pending retries screen.

![Pending Retries Tab](images/pending-retries.png 'width=500')


### Pending Retries Messages

The Pending Retries Messages screen shows failed messages which have been retried but the status of that retry is pending. The status of retried failures is updated when either the message is processed again as either an Audited message (success) or as a failed message.

Failed messages that are retried may stay in the pending state for the following reasons:
 - The retrying endpoint is not working (e.g. crashed or is scaled-out) and the retried messages is waiting in the queue and has not yet been processed.
 - The retrying endpoint does not have auditing enabled but has successfully processed the retried message.

To limit the time period of displayed messages select an option from the available predefined time periods, by default pending messages for all time are displayed.

![Period Filter](images/pending-retries-period-selection.png 'width=500')

Results can be filtered by queue name using the search functionality:

![Queue Filter](images/pending-retries-filter-queues.png 'width=500')

Displayed messages display information in the same way as on [Failed Message](intro-failed-messages.md).

 * **Failure Timestamp:** Timestamp when the message have failed.
 * **Endpoint** Processing endpoint.
 * **Machine** Machine on which the processing endpoint was operating.
 * **Redirect** Information if redirect is created for this queue.
 * **StackTrace:** Displays the full .NET exception stacktrace.
 * **Headers:** Displays a complete set of message headers.
 * **Body:** Displays the serialized message body.
 * **Copy Message Id:** Copies the message unique ID to the clipboard.
 * **Open in ServiceInsight:** Launches [ServiceInsight](/serviceinsight/), focusing on the message for in-depth analysis of the failure causes. This only works if ServiceInsight is installed on the local machine.

### Message Retry

WARNING: Pending retried messages can be retried however this feature should be used with care. It will create a duplicate of a message to be processed within the system. Person should ensure that this message was not processed or was processed incorrectly before using retry feature.

To retry a pending retry message, select the failed message(s) in the list and click the "Retry Selected" button.

Alternately a queue can be selected and "Retry All" option chosed which will retry all messages for that queue.

Message retry will use [message redirects](redirect.md) if the original endpoint has been redirected in ServicePulse.

WARNING: A pending retry message that is sent for retry will remain in the pending retry list until it is resolved or fails again.

### Mark as complete

When audits are disabled pending retries will remain in that state indefinitely, even when successfully processed. Use the `Mark as complete` feature to manually mark the failed message as resolved. Once the message is marked as completed it will no longer appear in the pending retries message list.