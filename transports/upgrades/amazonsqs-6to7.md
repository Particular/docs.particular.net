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

## V1 Compatibility Mode

The option to serialize the `TimeToBeReceived` and `ReplyToAddress` message headers in the message envelope for compatibility with endpoints using version 1 of the transport is no longer available.

Make sure that all V1 endpoints are upgraded to a supported version and remove the `transport.EnableV1CompatibilityMode();` configuration option.
