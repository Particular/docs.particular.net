---
title: AmazonSQS Transport Upgrade Version 5 to 6
summary: Instructions on how to upgrade the AmazonSQS transport from version 5 to 6
component: SQS
isUpgradeGuide: true
reviewed: 2022-03-25
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Delayed delivery

The unrestricted delayed delivery is now always enabled so the `UnrestrictedDurationDelayedDelivery()` API has been deprecated.

## Configuring the SQS transport

To use the SQS transport for NServiceBus, create a new `SqsTransport` instance and pass it to `EndpointConfiguration.UseTransport`.

Instead of:

snippet: 5to6-usage-old

Use:

snippet: 5to6-usage-new

include: v7-usetransport-shim-api

## SDK clients

Customizing instances of the SQS and SNS SDK clients is now done via the `SqsTransport` constructor.

Instead of:

snippet: 5to6-clients-old

Use:

snippet: 5to6-clients-new

## S3 configuration

Enabling S3 for handling large messages is now configured via the `S3` property of the transport definition.

NOTE: By default, the value is `null` which means S3 usage for sending large messages is disabled.

Instead of:

snippet: 5to6-S3-old

Use:

snippet: 5to6-S3-new

### Encryption

Message payload encryption is now configured via the `Encryption` property of the S3 settings object.

NOTE: By default, the value is `null` which means the messages are not encrypted.

Instead of:

snippet: 5to6-encryption-old

Use:

snippet: 5to6-encryption-new
