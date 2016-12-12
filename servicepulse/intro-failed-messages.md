---
title: Failed Message Monitoring
summary: Describes how ServicePulse detects and monitors failed messages, and allows retrying, or archiving failing messages
component: ServicePulse
tags:
- ServicePulse
reviewed: 2016-12-12
related:
- serviceinsight/managing-errors-and-retries
---

When an NServiceBus endpoint fails to process a message, it performs a set of configurable attempts to recover from this failure. These attempts are referred to as "immediate retries" and "delayed retries" and in many cases allow the endpoint to overcome intermittent communication failures. For more details see [recoverability](/nservicebus/recoverability/).

If the [recoverability](/nservicebus/recoverability/) attempts also fail, the endpoint forwards the failed message to the central error queue defined for all endpoints in the system. (See [Auditing with NServiceBus](/nservicebus/operations/auditing.md).)

ServicePulse (via ServiceControl) monitors the central error queue and displays the current status and details of failed messages as an indicator in the ServicePulse dashboard.

![Failed Messages indicator](images/indicators-failed-message.png 'width=500')


### Failed Messages Page

To see a detailed display of the failed messages, click the Failed Messages indicator (or the "Failed Messages" link in the navigation bar). This page is split into several tabs.

![Failed Message Groups Page](intro-failed-messages-failed-groups-page.png 'width=500')

#### Last 10 successful group retries

The list shows information about last 10 group retries that were performed containing following information:
 
 * **Title** expressing if it was group or set of messages.
 * **Retry request started** time indicating when the retry was initiated.
 * **Retry request completed** time indicating when the retry was completed.
 * **Messages retried** amount of messages that were retried.

NOTE: By writing completed retry the result of the retry is not being displayed. A completed retry means that messages were send to given queue.


#### Failed message groups

The first tab shows error groups. A group is a set of failed messages grouped by the same classification type.
There are following classifications that user can chose from:
 * **Exception Type and Stack Trace** groups messages by the same exception type and stack trace. It is the default way of categorizing failed messages.   
 * **Message Type** groups messages by its type. 
 
Regardless of classification each group has:

 * **Title** matching selected classification type.
 * **Count** of how many unresolved messages there are in the group.
 * **First Failure** time indicating when the first unresolved error occurred.
 * **Latest Failure** time indicating when the most recent unresolved errors occurred.
 * **Last Retried** time indicating when the group was retried.
 * **Actions** which can be used to Archive or Retry an entire group of messages (see below).

Click the  View Messages link to open a list of all of the errors within the group.
When the groups is being retried above information are replaced with the following ones:
 * **Title** name of the group that can have different names depending on classification type.
 * **Messages to send** amount of messages that will be retried after the operation is done
 * **Retry requested** time indicating when the retry was initiated.
 * **Progress indication** explaining where in the process of retrying the group currently is.

The second tab will display all failed messages. The functionality is the same as viewing the messages in a group.

![Failed Messages Page](intro-failed-messages-failed-messages-page.png 'width=500')

 * **Message Details:** For each failed message, displays the message type, exception description, endpoint name and location, and failure timestamp.
 * **StackTrace:** Displays the full .NET exception stacktrace.
 * **Headers:** Displays a complete set of message headers.
 * **Body:** Displays the serialized message body.
 * **Copy Message Id:** Copies the failed message unique ID to the clipboard.
 * **Open in ServiceInsight:** Launches [ServiceInsight](/serviceinsight/), focusing on the failed message for in-depth analysis of the failure causes. This only works if ServiceInsight is installed on the local machine.


### Failed Message Retry

After addressing the root cause of the message's processing failure, resend the failed message for reprocessing by the endpoint(s). This is referred to as a "retry" (or a manual retry, in contrast to the automated and configurable [Recoverability](/nservicebus/recoverability/)).

To retry a failed message, select the failed message(s) in the failed messages list and click the "Retry Selected" button (or click "Retry Group").

A message that is sent for retry is marked as such, and is not displayed in the failed message list or included in failed message groups, unless the reprocessing fails again. Messages sent for retry appear in the [Pending Retries screen](intro-pending-retries.md) until either an audit message is received indicating success, the message is returned as an error, or the message is manually marked as resolved.

If a message fails repeated retry attempts, an indication is added, including the number of times it has failed.

![Repeated failure indication](images/failed-messages-repeated-failure.png 'width=500')

NOTE: The number of retry attempts for a message can be significant if the handler for that message is not [idempotent](/nservicebus/concept-overview.md#idempotence). Any processing attempt that invokes logic that does not participate in the NServiceBus transactional processing will not be rolled back on processing failure.


### Archiving Failed Messages

Failed messages that cannot be processed successfully (or should not be retried due to various application-specific reasons) can be archived.

![Failed Message Archive](images/failed-messages-archive.png 'width=500')

Archiving in ServicePulse means that the failed messages are marked as "Archived". Its data is still available, but it is no longer displayed in the Failed Messages list in ServicePulse and is not counted by the Failed Messages indicator in the ServicePulse dashboard. It also will not appear in any failed message groups.

NOTE: Archived failed messages are included in [ServiceInsight](/serviceinsight/) diagrams and search results.
