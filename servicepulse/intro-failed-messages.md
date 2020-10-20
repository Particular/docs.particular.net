---
title: Failed Message Monitoring
summary: Describes how ServicePulse detects and monitors failed messages, and allows retrying, or deleting of failed messages
component: ServicePulse
reviewed: 2020-08-03
related:
- serviceinsight/managing-errors-and-retries
---

When an NServiceBus endpoint fails to process a message, it performs a set of configurable attempts to recover from message failure. These attempts are referred to as "immediate retries" and "delayed retries" and in many cases allow the endpoint to overcome intermittent communication failures. See [recoverability](/nservicebus/recoverability/) for more details.

If the automatic retry attempts also fail, the endpoint forwards the failed message to the error queue defined for all endpoints in the system. See [Auditing with NServiceBus](/nservicebus/operations/auditing.md) for more details.

ServicePulse (via ServiceControl) monitors the error queue and displays the current status and details of failed messages as an indicator in the ServicePulse dashboard.

![Failed Messages indicator](images/indicators-failed-message.png 'width=500')

Besides, ServicePulse also provides a Failed Messages page to assist in examining failed messages and taking specific actions on them.

## Failed Messages page

Both the "Failed Messages" indicator in the Dashboard and the "Failed Messages" link in the navigation bar link to the Failed Messages screen. This page is split into various tabs.

### Failed message groups tab

The first tab in the Failed Messages page shows error groups. A group is a set of failed messages grouped according to criterias like the same exception type.

This tab shows two lists, described below.

#### Last 10 completed retry requests list

This list is collapsed by default and shows information about the last ten completed group retry requests.

![Last 10 completed retry requests list](images/last-completed-group-retries.png 'width=500')

A completed retry request represents a completed operation where messages from a given group were sent to the corresponding queue for processing. This means those messages may not have been processed yet. [Learn more about retrying failed messages](/servicepulse/intro-failed-message-retries.md).


#### Failed groups list

This list shows all groups of currently failed messages.

![Failed Message Groups list](images/failed-message-groups.png 'width=500')

The display of failed message groups can be changed via the "Group by" drop-down menu, according to the following classification types:

 * **Exception Type and Stack Trace** - groups messages both by exception type and stack trace. It is the default way of categorizing failed messages.   
 * **Message Type** - groups messages by message type. 
 * **Endpoint Address** - groups messages by endpoint address where the failure occurred.
 * **Endpoint Instance** - groups messages by endpoint instance identifier where the failure occurred.
 * **Endpoint Name** - groups messages by name of the endpoint where the failure occurred.
 
Note: the number of listed groups may differ depending on the selected classifications type view.

##### Managing failed message groups

The following actions can be performed on a failed message group:

 * **View messages** - Shows all individual messages contained in the group.
 * **Request retry** - Sends all failed messages to the corresponding queue to attempt processing again. When a failed group retry request is initiated, ServicePulse will present the progress of the operation.

![Failed message groups retry in progress](images/failed-group-retry-in-progress.png 'width=500')

 * **Delete group** - Deletes all messages contained in the group. [Learn more about deleting messages](/servicepulse/intro-archived-messages.md).

### Listing messages

Individual failed messages can be viewed in one of the following two ways:

- **Inside a failed message group** - in the "Failed Messages Group" tab, click the "View messages" link from a failed message group entry
- **All messages without any grouping** - via the "All messages" tab

![Failed Messages Page](images/intro-failed-messages-failed-messages-page.png 'width=500')

Both of these message list views allow for taking actions on an individual message, on custom message selections or all messages contained in the view.

NOTE: Retrying one or a few individual messages can be useful for testing system fixes before deciding to retry several messages in a group. This is because retrying several messages take a long time and queue other ServiceControl operations for longer than desired.

The following actions can also be taken on each message or a selection of messages:

* **Retry** - Sends the message(s) to be reprocessed by the corresponding endpoint.
* **Delete** - Deletes message(s).

### Message details page

As of version 1.8.0 and above, each message can be browsed to see in-depth details about a given failed message, delete or to retry that message.

![Failed Messages Page](images/failed-message-page.png 'width=500')

Individual messages can be accessed by clicking the respective entry in any of the message list views.

Each individual failed message page allows for viewing the following additional message details:

* **Message metadata** - Failure timestamp, endpoint name and location, retry status.
* **StackTrace** - Full .NET exception stacktrace.
* **Headers** - Complete set of message headers.
* **Body** - Serialized message body.

The following actions can also be taken on any given message:

* **Retry** - Sends message to be [retried](/servicepulse/intro-failed-message-retries.md) by the corresponding endpoint.
* **Delete** - Deletes the message.
* **View in ServiceInsight** - Launches [ServiceInsight](/serviceinsight/), focusing on the failed message for in-depth analysis of the failure causes. This only works if ServiceInsight is installed on the local machine.

#### Sharing message data from ServicePulse

The URL from that message's page can be copied to share the details of a specific message from ServicePulse.

## Deleted Messages

### Deleted Message Groups

This list shows all groups of deleted messages.

![Deleted Message Groups Tab](images/archivegroups.png 'width=500')

The display of deleted message groups can be changed via the "Group by" drop-down menu, according to the following classification types:

 * **Exception Type and Stack Trace** - groups messages both by exception type and stack trace. It is the default way of categorizing failed messages.   
 * **Message Type** - groups messages by message type. 
 * **Endpoint Address** - groups messages by endpoint address where the failure occurred.
 * **Endpoint Instance** - groups messages by endpoint instance identifier where the failure occurred.
 * **Endpoint Name** - groups messages by name of the endpoint where the failure occurred.
 
Note: the number of listed groups may differ depending on the selected classifications type view.

### Deleted Messages

Failed messages that cannot be processed successfully (or could not be retried due to various application-specific reasons) can be deleted and later viewed in the Deleted Messages tab.

![Deleted Messages Tab](images/archive.png 'width=500')

Learn more about [deleting messages in ServicePulse](/servicepulse/intro-archived-messages.md).