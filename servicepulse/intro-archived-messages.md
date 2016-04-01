---
title: Archived Message Management in ServicePulse
summary: Describes how ServicePulse monitors archived failed messages, and allows un-archiving archived failed messages
tags:
- ServicePulse
reviewed:  
---

### Introduction

ServiceControl version 1.12.0 and higher has a configurable [ErrorRetentionPeriod](/servicecontrol/creating-config-file.md) that schedules the disposal of archived failed messages. In SerivcePulse 1.5.0 and higher screens to manage archived messages have been introduced.

Archived Messages can be found by going to the failed messages screen.

![Archived Messages Tab](images/archive.png)

### Archived Messages

The archived message page will open showing messages archive in the last 2 hours. Users can select from the available predefined ranges to limit the set of messages you are reviewing

![Archive Filters](images/archive-filters.png)

Each message displays when it is scheduled for deletion. Messages may display that they a scheduled for immediate deletion. This means they have expired but the scheduled task that deletes them from the data store hasn't run yet.

![Retention Countdown](images/archive-schedule.png)

By selecting individual messages and clicking the unarchive button an archived message can be returned back to the Failed Message Groups and Failed Messages screen where they can be retried or archived again.  

![Unarchive Select](images/archive-unarchive-select.png)

