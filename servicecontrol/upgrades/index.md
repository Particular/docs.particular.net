---
title: Upgrade Guides
summary: ServiceControl Upgrade Guides.
reviewed: 2020-03-16
related:
---


## Upgrade tips


1. Ensure the machine has sufficient CPU and RAM resources during upgrades. When hosted in a VM you can consider temporarily adding more CPU and RAM resources until the upgrade completes and afterwards trim down the resources.
1. Updating ServiceControl will stop ingesting error/audit queue messages but does not affect the sending of messages between endpoints. It will cause manual retries via ServicePulse to be unavailable. If uptime of ServicePulse is very important than consider [multiple (remote) instances](/servicecontrol/servicecontrol-instances/remotes.md).
1. Compacting the database will not make the update run faster. Compacting should only be considered *after* upgrading if storage.
1. For - I would recommend the in place update over the side-by-side update as it has less complexity. Consider side-by-side if minimal ServicePulse downtime is a must for your system.
1. Is is recommended to [backup the database](/servicecontrol/backup-sc-database.md) before upgrading.
1. Account for downtime of ServicePulse as failed messages can not be retried when ServiceControl is not running.
1. Account for queue storage size. While ServiceControl is down messages are not ingested. If the system has a high message throughput the queues act as a buffer. Ensure your queues do not run out of storage (quota) space as when queues are full no new messages can be added which will cause application outage.
1. Analyze the storage size for each instances and update the smallest to largest. This way you have an idea how long migrations run. With this info you can extrapolate how long updating your largest instances take and then you can always decide to for example go for a side-by-side upgrade.
