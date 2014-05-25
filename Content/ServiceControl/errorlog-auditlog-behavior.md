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

ServiceControl has also the ability to audit all messages that receives, this is required to support a scenario where auditing was previously used in the production environment and must be used even after the introduction of ServiceControl.

ServiceControl supports the ability to forward all messages that receives to a predefined audit queue called audit.log, this feature must be enabled after the ServiceControl installation.

In order to enable the audit.log queue one the the following approaches can be used:

* Add a configuration setting to the ServiceControl config file: `<add key="ServiceControl/ForwardAuditMessages" value="true" />` (see how to manage ServiceControl settings for detailed instructions)
* Add the following registry key to the registry of the machine where ServiceControl is running:
```
[HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceControl]
"ForwardAuditMessages"="true"
```