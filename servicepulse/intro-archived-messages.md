---
title: Deleted Message Management
summary: Describes how ServicePulse monitors deleted failed messages, and allows restoring deleted failed messages.
component: ServicePulse
reviewed: 2024-05-03
---

## About deleting messages

Archiving can be useful for failed messages that no longer have business value. Once a message is deleted, it can be automatically cleaned up according to the configuration of the [retention policy](/servicecontrol/servicecontrol-instances/configuration.md#data-retention-servicecontrolerrorretentionperiod).

Archiving in ServicePulse means that the failed messages are marked as `Deleted`. Data from a deleted message is still available, but it is no longer displayed in the Failed Messages list in ServicePulse, is not counted by the Failed Messages indicator in the ServicePulse dashboard, nor will it appear in any failed message groups.

> [!NOTE]
> Deleted failed messages are still included in [ServicePulse](/servicepulse/) diagrams and search results.

## Deleting messages

Messages can also be deleted from:

* the Failed Groups tab
* any failed group's contents view
* the All Messages tab
* the message details page

## Managing deleted messages

The Deleted Messages tab will open, showing messages deleted and not yet cleaned up according to the [retention policy](/servicecontrol/servicecontrol-instances/configuration.md#data-retention-servicecontrolerrorretentionperiod).

![Deleted Messages Tab](images/archive.png 'width=500')

To limit the set of displayed messages, select an option from the available predefined range.

![Delete Filters](images/archive-filters.png 'width=500')

Each message on screen contains information about when it's scheduled for deletion. "Immediate deletion" means that the message has expired, and will be deleted the next time the deletion task runs.

![Retention Countdown](images/archive-schedule.png 'width=500')

See [Service Control Error Retention Period](/servicecontrol/servicecontrol-instances/configuration.md#data-retention-servicecontrolerrorretentionperiod) to learn more about scheduling automatic disposal of deleted messages.

### Deleted Message Groups

The Deleted Message Groups tab shows the deleted messages grouped by the following options:

 * **Exception Type and Stack Trace** - groups messages both by exception type and stack trace. It is the default way of categorizing failed messages.
 * **Message Type** - groups messages by message type.
 * **Endpoint Address** - groups messages by endpoint address where the failure occurred.
 * **Endpoint Instance** - groups messages by endpoint instance identifier where the failure occurred.
 * **Endpoint Name** - groups messages by the name of the endpoint where the failure occurred.

> [!NOTE]
> The number of listed groups may differ depending on the selected classification type view.

Clicking on a group of messages will navigate to the Deleted Messages page containing all the messages from that group.

## Restoring deleted failed messages

If there are failed messages that were deleted by mistake, they can be restored via the Deleted Messages tab. Once restored, they will be displayed in the [Failed Message Groups and Failed Messages screen](intro-failed-messages.md), where they can be retried or deleted again.

![Restore Select](images/archive-unarchive-select.png)

### Restoring deleted groups of failed messages

Deleted groups of failed messages can also be restored by clicking on the respective "Restore group" button in the Deleted Message Groups tab.

![Restore group](images/deleted-group-restore.png 'width=500')
