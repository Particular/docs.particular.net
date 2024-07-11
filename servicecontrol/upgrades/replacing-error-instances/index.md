---
title: Replacing an Error instance
summary: Instructions on how to replace a ServiceControl Error instance with zero downtime
isUpgradeGuide: true
reviewed: 2024-07-10
related:
  - servicecontrol/upgrades/replacing-audit-instances
---

ServiceControl, which exists to serve the management of distributed systems, is itself a distributed system. As a result, pieces of the system can be upgraded and managed separately.

This document describes in general terms how to replace a ServiceControl Error instance, and links to more specific information on how to accomplish these tasks for each potential deployment method.

See [Replacing an Audit Instance](../replacing-audit-instances/) for similar guidance for Error instances.

## Step 1

TODO: Steal generic procedure from 4to5 upgrade guide