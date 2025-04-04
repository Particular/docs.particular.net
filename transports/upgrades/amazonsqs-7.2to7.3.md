---
title: AmazonSQS Transport Upgrade Version 7.2 to 7.3
summary: Instructions on how to upgrade the AmazonSQS transport from version 7.2 to 7.3
component: SQS
isUpgradeGuide: true
reviewed: 2025-04-04
upgradeGuideCoreVersions:
 - 9
---

The transport automatically extends the visibility timeout from message handling that takes longer. This ensures messages are not prematurely reprocessed if the handler execution time excees the visibility timeotu configured on the queue or on the receive request.

Without visibility renewal, long-running message handlers may experience:

- Duplicate processing due to expired visibility
- Message deletion failures, resulting in message reappearance
- Misleading delivery attempt counts for poison message detection

The message visibility timeout extension is enabled by default and will be attempted up to 5 minutes. It is possible to configure the message visibility timeout and the maximum time to extend it. For more information consult the transport configuration documentation.