---
title: Automatic Expiration for ServiceControl Data
summary: How to configure ServiceControl to automatically purge old data after a user-defined expiration period.
tags:
- ServiceControl
- Expiration
---

ServiceControl is a backend data service for ServicePulse production monitoring and ServiceInsight advanced debugging applications. It collects and stores all the traffic, in term of messages, that flows into the monitored system's endpoints (including audited messages, failed messages, status messages such as endpoint heartbeats, custom checks, and saga state). 

As such, ServiceControl serves as a recent data repository, and it is not intended to serve as a long-term archiving solution (for example: some data archiving policies require storing data for long periods measured in years; such requirements are met by dedicated long-term archiving tools).

ServiceControl implements a configurable data purging policy, removing audited messages that are older than a specified timespan. **Failed messages (including messages that are manually archived using ServicePulse's Archive feature) are not purged.** 

By default ServiceControl purges old data periodically, checking each minute and deleting data older than 30 days.

It is possible to control the above behavior using the following settings:

* ExpirationProcessTimerInSeconds: the default is once every 10 minutes
* HoursToKeepMessagesBeforeExpiring: the default is 30 days.

To change the ServiceControl behavior, you need to update its settings (see [Customizing ServiceControl configuration](creating-config-file.md)).

**Example:** check for expiration every 15 minutes and expire messages older than 10 days.

```bat
x:\Your_Installed_Path\ServiceControl.exe --restart -d=ServiceControl/ExpirationProcessTimerInSeconds==900 -d=ServiceControl/HoursToKeepMessagesBeforeExpiring==240
```

Note: It is not recommended to set ExpirationProcessTimerInSeconds to a value lower that 300 as this can have performance implicatons. 

## ServiceControl default installed location

By default ServiceControl is installed in `[Program Files]\Particular Software\ServiceControl`.
