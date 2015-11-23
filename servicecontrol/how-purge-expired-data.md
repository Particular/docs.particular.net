---
title: Automatic Expiration of ServiceControl Data
summary: Configuring ServiceControl data retention
tags:
- ServiceControl
- Expiration
---

ServiceControl contains audit information for a configurable period of time. Any audit data that is older than this threshold is deleted from the embedded RavenDB.
By default the expiration threshold is 30 days. This value can be modified via configuration.   
For more information on these settings refer to [Customizing ServiceControl configuration](creating-config-file.md)).

Note: The expiration process only curates the data in the embedded RavenDB. Audit and Error forwarding queues are not curated and or managed by ServiceControl 

**Example:**
 
The following settings would change the expiration task to run every 15 minutes and expire messages older than 10 days.

```
<add key="ServiceControl/ExpirationProcessTimerInSeconds" value=900 />
<add key="ServiceControl/HoursToKeepMessagesBeforeExpiring" value=240 />
```

Note: It is not recommended to set `ExpirationProcessTimerInSeconds` to a value lower that 300. 
