---
title: ServiceControl
summary: An overview of ServiceControl and what it does
reviewed: 2026-04-23
component: ServiceControl
isLearningPath: true
---

include: servicecontrol

Refer to [the Particular Service Platform](/platform/) article for more information on how ServiceControl and ServicePulse work together. Refer to the [how does ServiceControl work](/servicecontrol/how.md) article for a quick overview of the different instances, what they do, and how to configure endpoints.

### ServiceControl instance types

There are three types of instances that can be created:

- [Error instances](/servicecontrol/servicecontrol-instances/) are the most commonly used ServiceControl instance and are indispensable to ensure the smooth operation of an NServiceBus system. Together with ServicePulse (which can be [hosted by a ServiceControl Error instance](/servicecontrol/servicecontrol-instances/integrated-servicepulse.md)), they provide the ability to visualize and retry failed messages.
- [Audit instances](/servicecontrol/audit-instances/) provide valuable information about the message flow through a system. These instances are used by ServicePulse to help visualize a distributed system.
- [Monitoring instances](/servicecontrol/monitoring-instances/) provide performance monitoring and metrics analytics that are useful for keeping track of the health of a distributed system.

## Connecting endpoints to ServiceControl

NServiceBus endpoints must be configured to send data about their operations to a set of centralized queues. ServiceControl monitors these queues, then collects and processes the data from the NServiceBus endpoints.

> [!NOTE]
> Data is sent to queues even when ServiceControl is down. When ServiceControl becomes available, it will process the messages that were stored in the queue while it was offline.

To enable [ServiceControl](/servicecontrol) to gather this information, configure the endpoints appropriately:

- [Configure recoverability](/nservicebus/recoverability) to collect failed messages.
- [Enable auditing](/nservicebus/operations/auditing.md) to collect all messages.
- Install plugins to [monitor endpoint health](/monitoring/heartbeats/), collect [saga state changes](/nservicebus/sagas/saga-audit.md), and use [custom checks](/monitoring/custom-checks/).

See [_Optimizing ServiceControl for use in different environments_](/servicecontrol/servicecontrol-in-practice.md) for more information about practical considerations.
