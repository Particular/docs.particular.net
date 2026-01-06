---
title: Replacing an Error instance
summary: Instructions on how to replace a ServiceControl Error instance with zero downtime
reviewed: 2024-07-10
component: ServiceControl
related:
  - servicecontrol/migrations/replacing-audit-instances
---

ServiceControl, which exists to serve the management of distributed systems, is itself a distributed system. As a result, pieces of the system can be upgraded and managed separately.

This document describes in general terms how to replace a ServiceControl Error instance, and links to more specific information on how to accomplish these tasks for each potential deployment method.

See [Replacing an Audit Instance](../replacing-audit-instances/) for similar guidance for Error instances.

## Overview

ServiceControl Error instances serve as the central access point for both Error and Audit data. When ServicePulse requests data from the API, those requests are either answered by the Error instance directly or passed through to one or more Audit instances in a scatter/gather pattern. So it is possible to create a new Error instance configured to communicate with the same Audit instance(s) and begin using the new Error instance to consume messages from the error queue. The critical aspect is to protect any active error messages during the transition that might still contain any valuable business data.

Keeping this in mind, an Error instance that can't be upgraded can be replaced without downtime. The process follows these steps:

1. Disable error message ingestion so that new error messages will be temporarily held in the error queue.
2. Retry or archive any failed messages so that the old Error instance contains only ephemeral data like heartbeats and custom checks.
3. Create a new Error instance configured to use the same Audit instance(s).

## Getting ready

These steps should be followed before attempting to replace a ServiceControl Error instance:

1. All ServiceControl instances must be running version 4.33.0 or later. _This is required to support the upgrade path that keeps all failed messages safe._
2. In ServicePulse, clean up all [failed messages](/servicepulse/intro-failed-messages.md).
    * While not strictly necessary, a little "pre-cleaning" will ensure that fewer error messages need to be cared for during the upgrade, making the whole process go faster and more efficiently.
    * It's acceptable if a few failed messages still come in, but ideally, all failed messages should either be retried or archived.
    * It's best not to attempt this procedure during a failure incident when many error messages are incoming. Ideally, attempt the upgrade during a quiet period where no more than a handful of error messages are expected.

## Disable error message ingestion

The first step is to prevent any new error messages from entering the system. By disabling error ingestion on the Error instance, any new incoming error messages will be held safely in the error queue until the new Error instance can be created.

With ingestion stopped, the old Error instance will still be able to replay or archive messages currently in the database. If a replayed message succeeds when being processed by its source endpoint, the error is resolved and doesn't need any further action. If, however, the error reoccurs when being processed by its source endpoint, it will be sent back to the error queue, where it will be held until the new Error instance is established.

To disable error ingestion on the existing Error instance:

* [Disabling error ingestion with ServiceControl Management](scmu.md#disable-error-message-ingestion)
* [Disabling error ingestion with PowerShell](powershell.md#disable-error-message-ingestion)
* [Disabling error ingestion with Containers](containers.md#disable-error-message-ingestion)

## Retry or archive any failed messages

In ServicePulse, retry or archive any failed messages that have arrived during the upgrade process.

If a retried message fails again, it will go to the error queue, but the instance will not ingest it.

Once the failed message list is "clean" there will be no data of any continuing value left in the database, making it safe to continue. The only data left is ephemeral data, such as heartbeats, custom check results, etc.

## Replace the Error instance

Now that the only valuable error messages are held in the error queue, the Error instance can be replaced:

* [Replacing an Error instance with a Windows instance using ServiceControl Management](scmu.md#replace-the-error-instance)
* [Replacing an Error instance with a Windows instance using PowerShell](powershell.md#replace-the-error-instance)
* [Replacing an Error instance with a container-hosted instance](containers.md#replace-the-error-instance)

Once complete, the old Error instance has been successfully replaced with a new one.

If the procedure involved creating a new instance, rather than a forced upgrade, it will be necessary to adjust the connection information for ServicePulse to connect to the new Error instance's URL.