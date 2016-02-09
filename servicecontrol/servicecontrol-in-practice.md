---
title: ServiceControl in Practice
summary: Details how to think about the use of ServiceControl in production environments
tags:
- ServiceControl
---

ServiceControl is a tool that can accommodate many situations. Features can be enabled or disabled depending on the needs of the environment. Each feature is valuable in certain contexts but also comes with a cost in terms of resource usage and performance.

For example, in a development environment the [Debug Session](/servicecontrol/plugins/debug-session.md) and [Saga Audit](/servicecontrol/plugins/saga-audit.md) plugins provide detailed information to support problem analysis. Particular advise that customers not use these  plugin outside of a development environment.

Making decisions about the use of the other plugins and features requires a little more thought. The temptation to 'just use them all' as a catch-all insurance policy is not a good choice. 

- The cost of having certain features in play can actually cause serious performance issues without proper [capacity planning](/servicecontrol/capacity-and-planning.md). Particular recommend that in production environments there should only be one instance of ServiceControl per machine and that it should not share resources with other potentially resource hungry processes. For example, other NServiceBus Hosts, web services or SQL Server Instances. 
- The [Auditing](/nservicebus/operations/auditing.md) plugin will capture a record of everything that passes through an endpoint. It is worth considering turning audit off in some or all endpoints in production or directing the audit traffic to a null queue. If production audits are needed consider directing the traffic to a well resourced dedicated server. Audits in ServiceControl are primarily for performing system analysis using ServiceInsight. If ServiceInsight is not deployed in production then avoid using audits in production.
- [Heartbeats](/servicepulse/intro-endpoints-heartbeats.md) and [Custom Checks](/servicepulse/intro-endpoints-custom-checks.md) can enhance a system's monitoring capability, but they add extra noise. Often, not all endpoints are mission critical. Therefore it is worth making sure that the risks to the system if an endpoint is not available are examined and understood. All communication from endpoints with ServiceControl is performed via messaging. Adding a message to the queue every second may have little impact on when the notification shows up in ServicePulse. If ServiceControl is shutdown without pausing system Endpoints, heartbeats will continue to be added to the queue. ServiceControl will attempt to process all the messages in the queue when it restarts.

##### Less can be more

Moving forward, take a thoughtful approach in the adoption of plugins and features:

- Read the [Capacity Planning Guide](/servicecontrol/capacity-and-planning.md) for ServiceControl.
- Read the [Troubleshooting Guide](/servicecontrol/troubleshooting.md) before deployment in production.
- [Turn off auditing](/nservicebus/operations/auditing.md) on all endpoints as well as [heartbeats and custom checks](/servicepulse/how-to-configure-endpoints-for-monitoring.md).
- Perform load tests to baseline the solution.
- When comfortable with the performance of the system try adding [Heartbeats](/servicepulse/intro-endpoints-heartbeats.md) to monitor the system again.
- Try increasing the [heartbeat interval](/servicecontrol/plugins/heartbeat.md). Ideally heartbeat updates should not occur more frequently than ServiceControl can process them or more than Operations staff are prepared to monitor them with ServicePulse.
- With each additional change perform a load test again adjusting the heartbeat interval satisfied with the result.
- Repeat the process of considering the business relevance, system impact and load testing with each [Custom Check](/servicecontrol/plugins/custom-checks.md).
- Repeat the process of considering the business relevance, system impact and load testing with each Endpoint [Audit](/nservicebus/operations/auditing.md).

##### Tips and Tricks

- If running anti-virus software exclude the ServiceControl [database directory](/servicecontrol/configure-ravendb-location.md) from virus checks.
- Particular recommend that customers don't downgrade major and minor releases of ServiceControl. ServiceControl uses an embedded database and changes to the internal data structures can occur between releases. Rolling back may cause index corruption or data loss. Particular recommend testing in a lower environment before upgrading in production environments.