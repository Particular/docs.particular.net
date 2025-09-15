---
title: AmazonSQS Transport Upgrade Version 7 to 8
summary: Instructions on how to upgrade the AmazonSQS transport from version 7 to 8
component: SQS
reviewed: 2025-09-15
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
---

The transport now automatically extends the visibility timeout during message processing when the handler execution exceeds the configured timeout on the queue or receive request. This prevents messages from being prematurely reprocessed.

Without visibility renewal, long-running handlers may encounter:

- Duplicate processing due to expired visibility, wasting resources
- Message deletion failures, causing messages to reappear
- Misleading delivery attempt counters affecting poison message detection

By default, the message visibility extension is enabled and will be attempted for up to 5 minutes. Both the initial visibility timeout and the maximum extension duration are configurable. For more details, refer to the [transport configuration documentation](/transports/sqs/configuration-options.md#message-visibility).

The cancellation token available on the message handler context will be cancelled when the transport fails to renew the message visibility timeout. This makes it possible to cancel the message handling operations in cases when the message visibility was clearly lost and the current message being processed could not be successfully acknowledged.

## Visibility timeout subscription settings

Setting the message visibility timeout on the message driven pubsub compatibility mode has been deprecated in favour of the `MessageVisibilityTimeout` setting on the transport.

Updated Usage:
snippet: 7to8-sqs-message-visibility-timeout

## Upgraded the AWS SDK to version 4

The AWS SDK introduces [breaking changes](https://docs.aws.amazon.com/sdk-for-net/v4/developer-guide/net-dg-v4.html), such as returning null instead of empty collections. The persistence has been updated internally to accommodate these changes.

If your application code directly uses the AWS SDK and is affected by these changes, you’ll need to adjust your code accordingly by following the [AWS SDK v4 upgrade guidelines](https://docs.aws.amazon.com/sdk-for-net/v4/developer-guide/net-dg-v4.html).