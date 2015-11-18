---
title: Backup the ServiceControl Data
summary: How to backup the ServiceControl RavenDB Embedded instance
tags:
- ServiceControl
- RavenDB
- Backup
---
ServiceControl utilizes an embedded instance of RavenDB for data storage for each instance of the ServiceControl service. 
To backup a database instance, proceed as follows.

### Backup

1.  Open the ServiceControl Managaement Utility to view the list of ServiceControl Service instances
	![](managementutil-instance.png)
1. Stop the service you wish to backup from the action icons if it is running.        
1. Click the link under data path to go to the data directory. 
1. Copy the contents of the data directory. 
1. Start the service again once the copy is complete


### Restore

1. Open the ServiceControl Managaement Utility to view the list of ServiceControl Service instances
1. Stop the service you wish to restore to from the action icons.        
1. Click the link under data path to go to the data directory. 
1. Replace the contents of this directory with the previously copied data,  
1. Start the service again once the copy is complete


### Important Notes and Restrictions

While backup is in progress, the ServiceControl service must be stopped. During this time, ServiceControl data collection pauses and no connections can be made from ServicePulse, ServiceInsight, or any custom software that depends on the ServiceControl API endpoints.

Due to the nature of messaging, messages directed to ServiceControl remain in the queues until the ServiceControl service restarts, when normal message processing is restored and all the pending messages are processed, with no loss of information.

Care should be taken when planning to move ServiceControl from one server to another.  Moving databases between servers can be problematic. RavenDB does not support moving from a new versions of Windows back to older versions of Windows. See this [link](http://stackoverflow.com/questions/25625910/getting-error-while-restoring-backup-file-in-raven-db)

The ServiceControl database should not be restored to older copies of the ServiceControl service.  This is not supported as the both the database structure and the version on RavenDB can change between versions.  These changes aren't necessarily backward compatible.  


