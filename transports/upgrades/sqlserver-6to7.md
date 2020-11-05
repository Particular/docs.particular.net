---
title: SQL Server Transport Upgrade Version 6 to 7
reviewed: 2020-11-05
component: SqlTransport
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

Upgrading from SQL Server transport version 6 to version 7 is a major upgrade and requires careful planning. Read the entire guide before beginning the upgrade process.

## WithPeekDelay replaced by QueuePeekerOptions

In version 6 of the transport it was possible to define the message peek delay using the `WithPeekDelay` configuration option. The configuration setting has been moved to a more generic `QueuePeekerOptions` that allows to configure also other parameters related to message peeking.
