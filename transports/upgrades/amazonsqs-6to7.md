---
title: AmazonSQS Transport Upgrade Version 6 to 7
summary: Instructions on how to upgrade the AmazonSQS transport from version 6 to 7
component: SQS
isUpgradeGuide: true
reviewed: 2023-10-18
upgradeGuideCoreVersions:
 - 8
 - 9
---

## V1 Compatibility Mode

The option to serialize the `TimeToBeReceived` and `ReplyToAddress` message headers in the message envelope for compatibility with endpoints using version 1 of the transport is no longer available.

Make sure that all V1 endpoints are upgraded to a supported version and remove the `transport.EnableV1CompatibilityMode();` configuration option.
