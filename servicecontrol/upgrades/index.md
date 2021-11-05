---
title: Upgrade Guides
summary: ServiceControl Upgrade Guides.
reviewed: 2020-03-16
related:
---


## Upgrade tips


1. Ensure the machine that hosts ServiceControl has sufficient CPU and RAM resources during upgrades. Consider temporarily adding more vCPU and RAM until the upgrade completes and afterwards trim down the resources.
1. Updating ServiceControl will stop ingesting error/audit queue messages but does not affect the sending of messages between endpoints. During updating triggering manual retries via ServicePulse is not available. Consider [multiple (remote) instances](/servicecontrol/servicecontrol-instances/remotes.md) if uptime of ServicePulse is very important.
1. Compacting the database will not make the update run faster. Compacting should only be considered *after* upgrading.
1. In place update are recommended over side-by-side update as it has less complexity. Consider side-by-side if minimal ServicePulse downtime is a must.
1. It is recommended to [backup the database](/servicecontrol/backup-sc-database.md) before upgrading.
1. Account for downtime of ServicePulse as failed messages cannot be retried when ServiceControl is not running.
1. Account for queue storage size. While ServiceControl is down messages are not ingested. If the system has a high message throughput the queues act as a buffer. Ensure queues will not run out of storage (quota) space as when queues are full no new messages can be added which will cause application outage.
1. Analyze the storage size for each instances and update the smallest to largest. This helps understanding how not long migrations run and can be used to  extrapolate how long updating the largest instances will take and can help decided opting for a side-by-side upgrade.
