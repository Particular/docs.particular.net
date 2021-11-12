---
title: Upgrade Tips
summary: Tips for upgrading to newer versions of ServiceControl.
reviewed: 2020-03-16
related:
---

1. Ensure the machine that hosts ServiceControl has sufficient CPU and RAM resources during upgrades. Consider temporarily adding more vCPU and RAM until the upgrade completes and afterwards trim down the resources.
1. Updating ServiceControl will stop ingesting error/audit queue messages but does not affect the sending of messages between endpoints. While an update is in progress, it's not possible to trigger retries manually via ServicePulse. Consider [multiple (remote) instances](/servicecontrol/servicecontrol-instances/remotes.md) if uptime of ServicePulse is important.
1. Compacting the database will not make the update run faster. Compacting should only be considered *after* upgrading.
1. In-place updates are recommended over side-by-side updates as they have less complexity. Consider side-by-side if ServicePulse downtime must be minimized.
1. [Backup the database](/servicecontrol/backup-sc-database.md) before upgrading.
1. Account for downtime of ServicePulse as failed messages cannot be retried when ServiceControl is not running.
1. Account for queue storage size. While ServiceControl is down, messages are not ingested but they will still build up in the queue. If the system has a high message throughput the queues act as a buffer. Ensure queues will not run out of storage (quota) space as when queues are full no new messages can be added which will cause application outage.
1. Analyze the storage size for each ServiceControl instance and update starting from the smallest to the largest. This helps understand how smaller migrations run and can be used to extrapolate how long updating the larger instances will take which can help decided whether a side-by-side upgrade is necessary.
