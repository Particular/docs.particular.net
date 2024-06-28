---
title: Upgrade Tips
summary: Tips for upgrading to newer versions of ServiceControl.
reviewed: 2023-07-07
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

> [!NOTE]
> Installing a newer version of the ServiceControl Management utility or PowerShell will not upgrade each of the individual error, monitoring or audit instances. Each of those instances will need to be upgraded in a subsequent step separately if desired.

## Downgrading ServiceControl

This section contains information about downgrading ServiceControl.

> [!WARNING]
> ServiceControl instances can only be downgraded using a manual process. This is because the storage schema could have changed during a previous upgrade. If a downgrade MUST be performed ensure that the storage schema is compatible by reading the relevant [ServiceControl upgrade guides](/servicecontrol/upgrades/). It is recommended to perform downgrades under the supervision of a [Particular software support engineer](https://particular.net/support).

The method used to downgrade ServiceControl instances depends on the method used to deploy them:

### Downgrading using the ServiceControl Management utility

1. Open the ServiceControl Management Utility
2. Scroll to the instance that requires downgrading
3. Open the "Installation path"
4. Copy the `ServiceControl.exe.config` and store the identical instance name
5. Delete the instance but keep the database files
6. Close ServiceControl Management Utility
7. Uninstall the current (newer) version of ServiceControl from Windows in "Add/remove programs"
8. Download the `Particular.ServiceControl-<version>` exe file for the desired older version of the ServiceControl Management Utility from [the GitHub release note assets](https://github.com/Particular/ServiceControl/tags).
9. Install the older version of the ServiceControl Management Utility using the downloaded exe file.
10. Add a new instance using the same identical name and configuration as the deleted instance

> [!IMPORTANT]
> Downgrading the setup can results in SCMU not being able to manage newer instance versions.

### Downgrading using PowerShell

<!-- TODO -->

### Downgrading using containers

<!-- TODO -->
