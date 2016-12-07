---
title: Nsb6 ServiceControl Plugins Upgrade Version 1 to 2
summary: Instructions on how to upgrade NServiceBus version 6 ServiceControl Plugins Version 1 to 2.
reviewed: 2016-12-07
tags:
 - upgrade
 - migration
related:
- servicecontrol/plugins
- servicecontrol/plugins/heartbeat
- servicecontrol/plugins/custom-checks
- servicecontrol/plugins/saga-audit
---

## Connecting to ServiceControl

Version 2 of the ServiceControl plugins changes the way that the plugins connect to ServiceControl. The plugins no longer derive a ServiceControl queue name from the Error/Audit queues. Additional configuration is required to specify the location of the ServiceControl queue. 

### Configuration File

The location of the ServiceControl queue can be specified once for all plugins in via an `appSetting` in the endpoint configuration file.

snippet:sc-plugin-queue-config

### Code

The location of the ServiceControl queue can be specified via plugin-specific extensions to the endpoint configuration.

snippet:Heartbeats_Configure_ServiceControl

snippet:CustomCheck_Configure_ServiceControl

snippet:SagaAudit_Configure_ServiceControl