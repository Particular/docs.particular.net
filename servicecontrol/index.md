---
title: ServiceControl
reviewed: 2020-02-10
component: ServiceControl
isLearningPath: true
---

include: servicecontrol

For more information on how ServiceControl, ServicePulse, and ServiceInsight work together, refer to [the Particular Service Platform](/platform/).

### ServiceControl instance types

After [installation](/servicecontrol/installation.md), the ServiceControl Management Utility provides the ability to add, upgrade and delete ServiceControl instances. There are three different instances that can be created:

- [Error instances](/servicecontrol/servicecontrol-instances/)  
  This is the most commonly used ServiceControl instance and indispensable to ensure the correct working of an NServiceBus system. Together with SerivcePulse, it provides the ability to visualize and retry failed messages.
- [Audit instances](/servicecontrol/audit-instances/)  
  This ServiceControl Audit instance provides valuable information about the message flow through a system.
- [Monitoring instances](/servicecontrol/monitoring-instances/)  
  For performance monitoring and analyzing additional metrics, especially in production.

## Installation of ServiceControl instances

After downloading ServiceControl, the ServiceControl Management utility is available in the Windows Start menu. The ServiceControl instances mentioned above need to be installed and configured via the Management utility.

Read more about how to [install and configure](/servicecontrol/installation.md) ServiceControl instances.

## Connect endpoints to ServiceControl

NServiceBus endpoints need to be configured to send data about their operations to a set of centralized queues that are unique to the system. ServiceControl monitors these queues, collects and processes this data.

Note that the data is sent to queues, even when ServiceControl is down. When ServiceControl becomes available, it will process the messages that were stored in the queue while it was offline.

To enable [ServiceControl](/servicecontrol) to gather this information, configure the endpoints appropriately:

 * [Configure recoverability](/nservicebus/recoverability) to collect failed messages.
 * [Enable auditing](/nservicebus/operations/auditing.md) to collect all messages.
 * [Install plugins](/servicecontrol/plugins/) to monitor endpoint health, collect saga state changes, and use custom checks.

See [_Optimizing ServiceControl for use in different environments_](/servicecontrol/servicecontrol-in-practice.md) for more information about practical considerations.
