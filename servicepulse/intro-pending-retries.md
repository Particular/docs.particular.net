---
title: Pending Retries Message Management in ServicePulse
summary: Describes how ServicePulse monitors archived failed messages, and allows unarchiving archived failed messages.
tags:
 - ServicePulse
reviewed: 2016-07-28
---


In Versions **1.6.6** and above screens to manage pending retries messages have been introduced to display messages that are pending to be retried.

Pending retries messages can be found by going to the pending retries screen.

![Archived Messages Tab](images/archive.png 'width=500')


### Pending Retries Messages

The Pending Retries Messages page will open showing messages retried in the last 2 hours on which there was not reported status. Messages can be seen here when:
 - Endpoint is not working (crashed or is scaled-out) and retried messages is in the queue
 - Endpoint has audits turned off which prevents Service Control to mark the message as successfully handled.

To limit the set of displayed messages, select an option from the available predefined range.

![Archive Filters](images/archive-filters.png 'width=500')

Results can be filtered by queue name using the search functionality:

![Archive Filters](images/archive-filters.png 'width=500')

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

Pending retried messages can be retried however this feature should be used with care. It will create a duplicate of a message to be processed within the system. Person should ensure that this message was not processed or was processed incorrectly before using retry feature.

To retry a pending retry message, select the failed message(s) in the list and click the "Retry Selected" button.

A message that is sent for retry is marked as such, and is not displayed in the pending retry list when endpoint processing the message will send audit message or error message.

Message retry will use [redirect](redirect.md) if there is one present.

### Mark as complete

When audits are disabled ServiceControl can't automatically mark message as successfully processed. In that case manual `Mark as complete` is useful. After selecting pending retry message one should click "Mark as complete Selected" button. 

A message that is marked as completed will not show on pending retry message list.