---
title: Failed Message Monitoring
summary: Describes how ServicePulse detects and monitors failed messages, and allows retrying, or archiving failed messages
component: ServicePulse
reviewed: 2016-12-15
related:
- serviceinsight/managing-errors-and-retries
---

When an NServiceBus endpoint fails to process a message, it performs a set of configurable attempts to recover from this message failure. These attempts are referred to as "immediate retries" and "delayed retries" and in many cases allow the endpoint to overcome intermittent communication failures. See [recoverability](/nservicebus/recoverability/) for more details.

If the automatic retry attempts also fail, the endpoint forwards the failed message to the central error queue defined for all endpoints in the system. See [Auditing with NServiceBus](/nservicebus/operations/auditing.md) for more details.

ServicePulse (via ServiceControl) monitors the central error queue and displays the current status and details of failed messages as an indicator in the ServicePulse dashboard.

![Failed Messages indicator](images/indicators-failed-message.png 'width=500')

In addition, ServicePulse also provides a Failed Messages page to assist in examining failed messages and taking certain actions on them.


### Failed Messages page

Both the "Failed Messages" indicator in the Dashboard and the "Failed Messages" link in the navigation bar link to the Failed Messages screen. This page is split into various tabs.


#### Failed message groups tab

The first tab in the Failed Messages page shows error groups. A group is a set of failed messages grouped according to criteria like, for example, the same exception type.

This tab shows two lists, described below.


##### Last 10 completed retry requests list

This list is collapsed by default and shows information about the last 10 completed group retry requests.

![Last 10 completed retry requests list](images/last-completed-group-retries.png 'width=500')

A completed retry request represents a completed operation where messages from a given group were sent to the corresponding queue for processing. This means those messages may not actually have been processed yet. [Learn more about retrying failed messages](/servicepulse/intro-failed-message-retries.md).


##### Failed groups list

This list shows all groups of currently failed messages.

![Failed Message Groups list](images/failed-message-groups.png 'width=500')

The display of failed message groups can be changed via the "Group by" drop down menu, according to the following classifications types:

 * **Exception Type and Stack Trace** - groups messages both by exception type and stack trace. It is the default way of categorizing failed messages.   
 * **Message Type** - groups messages by message type. 
 
Note: the number of listed groups may differ depending on the selected classifications type view.


###### Managing failed message groups

The following actions can be performed on a failed message group:

 * **View messages** - Shows all individual messages contained in the group.
 * **Request retry** - Sends all failed messages to the corresponding queue to attempt processing again. When a failed group retry request is initiated, ServicePulse will present the progress of the operation.

![Failed message groups retry in progress](images/failed-group-retry-in-progress.png 'width=500')

 * **Archive group** - Archives all messages contained in the group. [Learn more about archiving messages](/servicepulse/intro-archived-messages.md).


#### Viewing individual messages

Individual messages views allow for accessing in-depth details about a given failed message, or retrying individual messages.

Individual failed messages can viewed using one of the following two ways:

- **Inside a failed message groups** - in the "Failed Messages Group" tab, click the "View messages" link from a failed message group entry
- **All messages without any grouping** - in the "All messages" tab

![Failed Messages Page](images/intro-failed-messages-failed-messages-page.png 'width=500')

Both of these individual message list views allow for taking actions on an individual message, on custom message selections or on all messages contained in the view.

NOTE: Retrying one or a few individual messages can be useful for testing system fixes before deciding to retry several messages in a group. This is because retrying several messages take a long time and queue other ServiceControl operations for longer than desired.


##### Managing individual failed messages

The following actions can be taken on each individual message:

 * **Message Details:** For a given failed message, displays the message type, exception description, endpoint name and location, and failure timestamp.
 * **StackTrace:** Displays the full .NET exception stacktrace.
 * **Headers:** Displays a complete set of message headers.
 * **Body:** Displays the serialized message body.
 * **Copy Message Id:** Copies the failed message unique ID to the clipboard.
 * **Open in ServiceInsight:** Launches [ServiceInsight](/serviceinsight/), focusing on the failed message for in-depth analysis of the failure causes. This only works if ServiceInsight is installed on the local machine.


### Archived Messages tab

Failed messages that cannot be processed successfully (or should not be retried due to various application-specific reasons) can be archived.

![Archived Messages Tab](images/archive.png 'width=500')

Learn more about [archiving messages in ServicePulse](/servicepulse/intro-archived-messages.md).

