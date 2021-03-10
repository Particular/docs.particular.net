---
title: Upgrade AmazonSQS Transport Version 5 to 6
summary: Instructions on how to upgrade the AmazonSQS transport from version 5 to 6
component: SQS
isUpgradeGuide: true
reviewed: 2021-03-01
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Delayed delivery

The unrestricted delayed delivery is now always enabled so the `UnrestrictedDurationDelayedDelivery()` API has been deprecated.


## Configuring the SQS transport

To use the SQS transport for NServiceBus, create a new `SqsTransport` instance and pass it to `EndpointConfiguration.UseTransport`.

Instead of

```csharp
var transport = endpointConfiguration.UseTransport<SqsTransport>();
```

Use:

```csharp
var transport = new SqsTransport();
endpointConfiguration.UseTransport(transport);
```

## SDK clients

In order to pass customized instances of the SQS and SNS SDK clients to the transport use the corresponding `SqsTransport` constructor overload.

snippet: 5to6-clients

## S3 configuration

The S3 usage for large messages is configured via the `S3` property of the transport. By default, the value is `null` which means S3 usage for sending large messages is disabled.

snippet: 5to6-S3

### Encryption

Message payload encryption is configured via the `Encryption` property of the S3 settings object. By default, the value is `null` which means the messages are not encrypted.

snippet: 5to6-encryption

## Configuration options

The SQS transport configuration options that have not changed have been moved to the `SqsTransport` class. See the following table for further information:

| Version 6 configuration option | Version 7 configuration option |
| --- | --- |
| EnableV1CompatibilityMode | EnableV1CompatibilityMode |
| MapEvent | MapEvent |
| MaxTimeToLive | MaxTimeToLive |
| Policies | Policies |
| TopicNameGenerator | TopicNameGenerator |
| TopicNamePrefix | TopicNamePrefix |
| QueueNamePrefix | QueueNamePrefix |
| Policies.AddAccountCondition | Policies.AccountCondition |
| Policies.AddTopicNamePrefixCondition | Policies.TopicNamePrefixCondition |
| Policies.AddNamespaceCondition | Policies.TopicNamespaceConditions |
| Policies.AssumePolicyHasAppropriatePermissions | Policies.SetupTopicPoliciesWhenSubscribing |
