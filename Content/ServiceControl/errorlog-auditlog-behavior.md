---
title: ServiceControl error.log and audit.log behavior
summary: Details how ServiceControl handles internal errors and auditing
tags:
- ServiceControl
- Error
- Audit
---
As any regular endpoint, in a messaging based system ServiceControl has its own failure protection based on error queues.

By default ServiceControl, at install time, creates a queue named `error.log` where all the messages that ServiceControl fails to process flows.

ServiceControl has also the ability to audit all messages that receives, this is required to support a scenario where auditing was previously used in the system and must be used even after the introduction of ServiceControl







To enable the forwarding of audit messages you can:
- Add `<add key="ServiceControl/ForwardAuditMessages" value="true" />` to config file
- Or add 
```
[HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceControl]
"ForwardAuditMessages"="true"
```
to the registry