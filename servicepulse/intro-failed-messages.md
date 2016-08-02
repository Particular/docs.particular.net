---
title: 'ServicePulse: Failed Message Monitoring' 
summary: Describes how ServicePulse detects and monitors failed messages, and allows retrying, or archiving failing messages
tags:
- ServicePulse
reviewed: 2016-03-02
related:
- serviceinsight/managing-errors-and-retries
---

When an NServiceBus endpoint fails to process a message, it performs a set of configurable attempts to recover from this failure. These attempts are referred to as "immediate retries" and "delayed retries" and in many cases allow the endpoint to overcome intermittent communication failures. For more details see [recoverability](/nservicebus/recoverability/).

If the [recoverability](/nservicebus/recoverability/) attempts also fail, the endpoint forwards the failed message to the central error queue defined for all endpoints in the system. (See [Auditing with NServiceBus](/nservicebus/operations/auditing.md).)

ServicePulse (via ServiceControl) monitors the central error queue and displays the current status and details of failed messages as an indicator in the ServicePulse dashboard.

![Failed Messages indicator](images/indicators-failed-message.png 'width=500')


### Failed Messages Page

To see a detailed display of the failed messages, click the Failed Messages indicator (or the "Failed Messages" link in the navigation bar). This page is split into two tabs.

![Failed Message Groups Page](intro-failed-messages-failed-groups-page.png 'width=500')

The first tab shows error groups. A group is a set of failed messages where the same **Exception Type** has been thrown from the same method. Each group has:

 * **Title** made up of the **Exception Type** and **Call Site** where the failure occurred.
 * **Count** of how many unresolved messages there are in the group.
 * **First Failure** time indicating when the first unresolved error occurred.
 * **Latest Failure** time indicating when the most recent unresolved errors occurred.
 * **Actions** which can be used to Archive or Retry an entire group of messages (see below).

Click the title of a group or the View Messages link to open a list of all of the errors within the group.

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

A message that is sent for retry is marked as such, and is not displayed in the failed message list or included in failed message groups, unless the reprocessing fails again.

If a message fails repeated retry attempts, an indication is added, including the number of times it has failed.

![Repeated failure indication](images/failed-messages-repeated-failure.png 'width=500')

NOTE: The number of retry attempts for a message can be significant if the handler for that message is not [idempotent](/nservicebus/concept-overview.md#idempotence). Any processing attempt that invokes logic that does not participate in the NServiceBus transactional processing will not be rolled back on processing failure.


### Archiving Failed Messages

Failed messages that cannot be processed successfully (or should not be retried due to various application-specific reasons) can be archived.

![Failed Message Archive](images/failed-messages-archive.png 'width=500')

Archiving in ServicePulse means that the failed messages are marked as "Archived". Its data is still available, but it is no longer displayed in the Failed Messages list in ServicePulse and is not counted by the Failed Messages indicator in the ServicePulse dashboard. It also will not appear in any failed message groups.

NOTE: Archived failed messages are included in [ServiceInsight](/serviceinsight/) diagrams and search results.