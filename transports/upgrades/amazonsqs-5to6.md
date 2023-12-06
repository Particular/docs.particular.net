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

```csharp
var transport = endpointConfiguration.UseTransport<SqsTransport>();
```

Use:

```csharp
var transport = new SqsTransport();
endpointConfiguration.UseTransport(transport);
```

include: v7-usetransport-shim-api

## SDK clients

In order to pass customized instances of the SQS and SNS SDK clients to the transport use the corresponding `SqsTransport` constructor overload.

snippet: 5to6-clients

## S3 configuration

Enabling S3 for handling large messages is now configured via the `S3` property of the transport definition.

NOTE: By default, the value is `null` which means S3 usage for sending large messages is disabled.

snippet: 5to6-S3-new

Old syntax:

snippet: 5to6-S3-old

### Encryption

Message payload encryption is now configured via the `Encryption` property of the S3 settings object.

NOTE: By default, the value is `null` which means the messages are not encrypted.

snippet: 5to6-encryption-new

Old syntax:

snippet: 5to6-encryption-old
