---
title: Archived Message Management
summary: Describes how ServicePulse monitors archived failed messages, and allows unarchiving archived failed messages.
component: ServicePulse
reviewed: 2020-08-03
---

## About message archival

In Versions 1.5.0 and above, a screen to manage archived messages has been introduced in response to changes in ServiceControl relating to Archived Message retention and disposal.

Archiving can be useful for failed messages that no longer have business value. Once a message is archived it can be automatically cleaned up according to the configuration of the [retention policy](/servicecontrol/creating-config-file.md#data-retention-servicecontrolhourstokeepmessagesbeforeexpiring).

Archiving in ServicePulse means that the failed messages are marked as `Archived`. Data from an archived message is still available, but it is no longer displayed in the Failed Messages list in ServicePulse, is not counted by the Failed Messages indicator in the ServicePulse dashboard, nor will it appear in any failed message groups.

NOTE: Archived failed messages are still included in [ServiceInsight](/serviceinsight/) diagrams and search results.

## Archiving messages

Archived messages can be found in ServicePulse in a tab in the Failed Messages page. From the Failed Messages page, messages can also be archived from:

* Failed Groups tab
* Any failed group's contents view
* All Messages tab
* The message details page

## Managing archived messages

The Archived Messages tab will open showing messages archived and not yet cleaned up according to the [retention policy](/servicecontrol/creating-config-file.md#data-retention-servicecontrolhourstokeepmessagesbeforeexpiring).

![Archived Messages Tab](images/archive.png 'width=500')

To limit the set of displayed messages, select an option from the available predefined range.

![Archive Filters](images/archive-filters.png 'width=500')

Each message on screen contains information about when it's scheduled for deletion. "Immediate deletion" means that the message has expired, and will be deleted the next time the deletion task runs.

![Retention Countdown](images/archive-schedule.png 'width=500')

See [Service Control Error Retention Period](/servicecontrol/creating-config-file.md) to learn more about scheduling automatic disposal of archived messages.

### Archived Message Groups

The Archived Message Groups tab shows the archived messages grouped by the following options:

 * **Exception Type and Stack Trace** - groups messages both by exception type and stack trace. It is the default way of categorizing failed messages.   
 * **Message Type** - groups messages by message type. 
 * **Endpoint Address** - groups messages by endpoint address where the failure occurred.
 * **Endpoint Instance** - groups messages by endpoint instance identifier where the failure occurred.
 * **Endpoint Name** - groups messages by name of the endpoint where the failure occurred.
 
Note: the number of listed groups may differ depending on the selected classifications type view.

Clicking on a group of messages will navigate to the Archived Messages page containing all the messages from that group.

## Unarchiving failed messages

If there are failed messages that were archived by mistake, they can be unarchived via the Archived Messages tab. Once unarchived, they will be displayed in the [Failed Message Groups and Failed Messages screen](intro-failed-messages.md), where they can be retried or archived again.

![Unarchive Select](images/archive-unarchive-select.png)