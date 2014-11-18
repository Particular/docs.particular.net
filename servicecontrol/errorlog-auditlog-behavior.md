---
title: ServiceControl error.log and audit.log behavior
summary: Details ServiceControl logging feature
tags:
- ServiceControl
- Error
- Audit
- Logging
---
When ServiceControl is installed and configured it starts monitoring `error` and `audit` queues and all messages flowing into these queues are processed by ServiceControl and stored into its internal database.

The consumed messages are available via other Platform tools such as ServicePulse and ServiceInsight that can connect to the ServiceControl API to retrieve information regarding the consumed messages and the processes running in the system.

Given the above behavior, if the system where we are installing ServiceControl already relies on `error` and `audit` queues to work properly, we can leverage the ServiceControl logging feature to configure ServiceControl to forward each consumed message to a specific queue in order to preserve the existing functionality.

By default ServiceControl, at install time, creates a queue named `error.log`, each time ServiceControl consumes a message from the `error` queue the message is also forwarded to the `error.log` queue.

### Auditing

ServiceControl messages can also be audited, e.g. to support a scenario where auditing was previously used in the production environment and must be used even after the introduction of ServiceControl.

ServiceControl supports the ability to forward all messages that receives to a predefined audit queue called `audit.log`, this feature must be enabled after the ServiceControl installation.

In order to enable the `audit.log` queue one of the following approaches can be used:

* Add a configuration setting to the ServiceControl configuration file: `<add key="ServiceControl/ForwardAuditMessages" value="true" />` (see how to manage ServiceControl settings for detailed instructions)
* Add the following registry key to the registry of the machine where ServiceControl is running:
```
[HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceControl]
"ForwardAuditMessages"="true"
```