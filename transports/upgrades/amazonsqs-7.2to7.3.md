---
title: AmazonSQS Transport Upgrade Version 7.2 to 7.3
summary: Instructions on how to upgrade the AmazonSQS transport from version 7.2 to 7.3
component: SQS
isUpgradeGuide: true
reviewed: 2025-04-04
upgradeGuideCoreVersions:
 - 9
---

The transport now automatically extends the visibility timeout during message processing when the handler execution exceeds the configured timeout on the queue or receive request. This prevents messages from being prematurely reprocessed.

Without visibility renewal, long-running handlers may encounter:

- Duplicate processing due to expired visibility, wasting resources
- Message deletion failures, causing messages to reappear
- Misleading delivery attempt counters affecting poison message detection

By default, the message visibility extension is enabled and will be attempted for up to 5 minutes. Both the initial visibility timeout and the maximum extension duration are configurable. For more details, refer to the [transport configuration documentation](/transports/sqs/configuration-options.md#message-visibility).
