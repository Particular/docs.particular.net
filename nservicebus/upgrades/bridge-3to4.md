---
title: Messaging Bridge Upgrade Version 3 to 4
component: Bridge
reviewed: 2024-09-12
isUpgradeGuide: true
---

## All endpoints must be registered with the bridge

Version 4 of the Messaging Bridge will default to translating the `NServiceBus.ReplyToAddress` message header for all messages.
This is a change from previous bridge versions, in which the translation of the header had to be explicitly enabled for failed messages.

This behaviour ensures successful delivery of in-flight messages between endpoints that have changed transports during the lifetime of the message.

> [!NOTE]
> The `TranslateReplyToAddressForFailedMessages()` method is now obsolete and will be removed in the next major version of the bridge. It is necessary to remove invocations of the method after upgrading to version 4.

Since the header may contain a physical address of any endpoint in the system, the change in the behavior requires all endpoints in the system to be registered with the bridge. This ensures that the information necessary for the `NServiceBus.ReplyToAddress` header translation is available when messages are transferred by the bridge.

> [!WARNING]
> If the `NServiceBus.ReplyToAddress` header cannot be translated by the bridge (because the endpoint it refers to is not registered), then the message
 will be moved to [the bridge error queue](/nservicebus/bridge/configuration.md#recoverability-error-queue). To prevent the need for retrying messages from the bridge error queue (which requires a custom, transport-specific solution) it is strongly recommended to register all endpoints in the system **before** upgrading to version 4.

Users who still want to prevent the need to register all endpoints with the bridge can use a dedicated API option:

snippet: do-not-translate-reply-to-address-for-failed-messages

