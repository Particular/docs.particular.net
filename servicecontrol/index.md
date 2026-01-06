---
title: ServiceControl
summary: An overview of ServiceControl and what it does
reviewed: 2024-07-19
component: ServiceControl
isLearningPath: true
---

include: servicecontrol

For more information on how ServiceControl and ServicePulse work together, refer to [the Particular Service Platform](/platform/) article. For a quick overview of the different instances, what they do and how to configure endpoints, refer to the [how does ServiceControl work](/servicecontrol/how.md) article.

### ServiceControl instance types

There are three types of instances that can be created:

- [Error instances](/servicecontrol/servicecontrol-instances/)
  This is the most commonly used ServiceControl instance and indispensable to ensure the smooth operation of an NServiceBus system. Together with ServicePulse, it provides the ability to visualize and retry failed messages.
- [Audit instances](/servicecontrol/audit-instances/)
  Audit instances provide valuable information about the message flow through a system. Among other things, this is used by ServicePulse to help visualize a distributed system.
- [Monitoring instances](/servicecontrol/monitoring-instances/)
  Monitoring instances performance monitoring and analyzing additional metrics and are useful for keeping track of the health of a distributed system.

## Connect endpoints to ServiceControl

NServiceBus endpoints must be configured to send data about their operations to a set of centralized queues that are unique to the system. ServiceControl monitors these queues, then collects and processes the data from the NServiceBus endpoints.

Note that the data is sent to queues, even when ServiceControl is down. When ServiceControl becomes available, it will process the messages that were stored in the queue while it was offline.

To enable [ServiceControl](/servicecontrol) to gather this information, configure the endpoints appropriately:

- [Configure recoverability](/nservicebus/recoverability) to collect failed messages.
- [Enable auditing](/nservicebus/operations/auditing.md) to collect all messages.
- Install plugins to [monitor endpoint health](/monitoring/heartbeats/), collect [saga state changes](/nservicebus/sagas/saga-audit.md), and use [custom checks](/monitoring/custom-checks/).

See [_Optimizing ServiceControl for use in different environments_](/servicecontrol/servicecontrol-in-practice.md) for more information about practical considerations.
