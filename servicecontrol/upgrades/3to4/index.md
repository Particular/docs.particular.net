---
title: Upgrade ServiceControl from Version 3 to Version 4
summary: Instructions on how to upgrade ServiceControl from version 3 to 4
isUpgradeGuide: true
---

## Overview

Upgrading ServiceControl from version 3 to version 4 is a major upgrade and requires careful planning. During the upgrade process, the instance of ServiceControl that is being upgraded will no longer be available and will not be ingesting any messages.

## Prerequisites

Before upgrading to ServiceControl version 4 the instance being upgraded must be upgraded to at least [version 3.8.2](https://github.com/Particular/ServiceControl/releases/tag/3.8.2).

For more information how to upgrade from Version 1.x to 3.8.2 consult the [upgrade documentation](/servicecontrol/upgrades/).

## ServiceControl Audit

ServiceControl version 4 introduces a new separate process to handle the audit queue. This Audit Instance reads messages from the audit queue, stores them in it's internal database, and (optionally) forwards the processed messages to an audit log queue.

The original ServiceControl instance will no longer ingest messages from the audit queue. It can still contain audit messages that have already been ingested. These messages will be retained until the configured audit retention period has passed.

This split is transparent to the other components of the Particular Software Platform, which should continue to connect to the main ServiceControl instance.

There are 3 possible outcomes from upgrading a ServiceControl instance to version 4. These outcomes depend on the configuration of the instance at the time of the upgrade.

- If the instance is configured to ingest messages from the **audit** queue **and** the **error** queue, it will be [split into two separate processes](./split.md).
- If the instance is configured to ingest messages from the **audit** queue, **but not** the **error** queue, it will be [converted into a ServiceControl Audit instance](./convert.md).
- If the instance is configured to inegst messages from the **error** queue, **but not** the **audit** queue, it will be [directly upgraded](./direct.md).


