---
title: Upgrade AmazonSQS Transport Version 1 to 3
summary: Instructions on how to upgrade the AmazonSQS transport from version 1 to 3.
component: SQS
isUpgradeGuide: true
reviewed: 2018-05-14
upgradeGuideCoreVersions:
 - 6
---

For customers on version 1 of the transport it is recommended they update directly to version 3 of the transport instead of updating to version 2.

## New Configuration API

In version 1 the transport was configured via connection string parameters. Connection strings have been removed in favor of a code based configuration API.

The new configuration API is accessible through extension methods on the `UseTransport<SqsTransport>()` extension point in the endpoint configuration. Refer to the [Full Configuration Page](/transports/sqs/configuration-options.md) for more details.

include: amazonsqs-xto3

## Wire compatibility with 1.x endpoints

Versions 2 and 3 of the transport break wire compatibility with version 1 endpoints. The `TimeToBeReceived` and `ReplyToAddress` properties are no longer present in the message envelope, but instead are available in the message headers. Starting with version 3.3.0 of the transport a setting has been introduced to enable wire compatibility with 1.x endpoints when needed, at the expense of larger message size. To do so use the [`EnableV1CompatibilityMode` setting](/transports/sqs/configuration-options.md#v1-compatibility-mode):

snippet: V1BackwardsCompatibility
