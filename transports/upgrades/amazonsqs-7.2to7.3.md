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

The cancellation token available on the message handler context will be cancelled when the transport fails to renew the message visibility timeout. This makes it possible to cancel the message handling operations in cases when the message visibility was clearly lost and the current message being processed could not be successfully acknowledged.

## Visibility timeout subscription settings

Setting the message visibility timeout on the message driven pubsub compatibility mode has been deprecated in favour of the `MessageVisibilityTimeout` setting on the transport.

Replace

```csharp
var migrationSettings = routing.EnableMessageDrivenPubSubCompatibilityMode();
migrationSettings.MessageVisibilityTimeout(timeoutInSeconds: 10);
```

with

```csharp
var transport = new SqsTransport
{
    MessageVisibilityTimeout = TimeSpan.FromSeconds(10)
};
```