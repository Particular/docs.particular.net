---
title: Archived Message Management
summary: Describes how ServicePulse monitors archived failed messages, and allows unarchiving archived failed messages.
component: ServicePulse
tags:
 - ServicePulse
reviewed: 2016-12-15
---


In Versions 1.5.0 and above a screen to manage archived messages has been introduced in response to changes in ServiceControl relating to Archived Message retention and disposal.

Archived Messages can be found in a tab in the Failed Messages page. From the Failed Messages page, messages can also be archived from the Failed Groups tab, from the list of message corresponding to a certain Failed Group, and from the All Messages view.

Archiving in ServicePulse means that the failed messages are marked as "Archived". Data from an archived message is still available, but it is no longer displayed in the Failed Messages list in ServicePulse and is not counted by the Failed Messages indicator in the ServicePulse dashboard. It also will not appear in any failed message groups.

ServiceControl version 1.13.0 and above has a configurable [ErrorRetentionPeriod](/servicecontrol/creating-config-file.md) that schedules the disposal of archived failed messages.

![Archived Messages Tab](images/archive.png 'width=500')

The Archived Messages tab will open showing messages archived in the last 2 hours. To limit the set of displayed messages, select an option from the available predefined range.

![Archive Filters](images/archive-filters.png 'width=500')

Each message on screen contains information about when it's scheduled for deletion. "Immediate deletion" means that the message has expired, and will be deleted the next time the deletion task runs. See also [ServiceControl Error Retention Period](/servicecontrol/creating-config-file.md).

![Retention Countdown](images/archive-schedule.png 'width=500')

NOTE: Archived failed messages are included in [ServiceInsight](/serviceinsight/) diagrams and search results.

## Unarchiving failed messages

Archived messages may be unarchived in the Archived Messages tab. It will be then displayed in the [Failed Message Groups and Failed Messages screen](intro-failed-messages.md), where it can be retried or archived again.

![Unarchive Select](images/archive-unarchive-select.png)