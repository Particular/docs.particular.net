---
title: Archived Message Management in ServicePulse
summary: Describes how ServicePulse monitors archived failed messages, and allows unarchiving archived failed messages.
tags:
 - ServicePulse
reviewed: 2016-04-06
---


In Versions 1.5.0 and above screens to manage archived messages have been introduced in response to changes in ServiceControl relating to Archived Message retention and disposal.

ServiceControl version 1.13.0 and above has a configurable [ErrorRetentionPeriod](/servicecontrol/creating-config-file.md) that schedules the disposal of archived failed messages.

Archived messages can be found by going to the failed messages screen.

![Archived Messages Tab](images/archive.png)


### Archived Messages

The Archived Messages page will open showing messages archived in the last 2 hours. To limit the set of displayed messages, select an option from the available predefined range.

![Archive Filters](images/archive-filters.png)

Each message on screen contains information about when it's scheduled for deletion. "Immediate deletion" means that the message has expired, and will be deleted the next time the deletion task runs. See also [Service Control Error Retention Period](/servicecontrol/creating-config-file.md).

![Retention Countdown](images/archive-schedule.png)

The archived message may be unarchived by clicking the Unarchive button. It will be then displayed in the [Failed Message Groups and Failed Messages screen](intro-failed-messages.md), where it can be retried or archived again.
![Unarchive Select](images/archive-unarchive-select.png)
