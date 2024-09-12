---
title: Messaging Bridge Upgrade Version 3 to 4
component: Bridge
reviewed: 2024-09-12
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
 - 9
---

## ReplyToAddress header translation for failed messages

Version 4 of the Messaging Bridge will translate the `NServiceBus.ReplyToAddress` message header for failed messages.
This is a change from previous versions of the bridge, which defaulted to not translating this header.

> [!NOTE]
> The `TranslateReplyToAddressForFailedMessages()` method is now obsolete and will be removed in the next major version of the bridge.  Calls to this method should be removed

Since any endpoint can be the source of a failed message, the consequence of this change in behavior is that the all endpoints need to be registered with the bridge so that the `NServiceBus.ReplyToAddress` header can be translated.

> [!WARNING]
> If the `NServiceBus.ReplyToAddress` header cannot be translated by the bridge because the endpoint it refers to is not registered, then the failed messages
 will be moved to the `bridge.error` queue instead of the `error` queue that feeds in to ServiceControl.  Any messages in the `bridge.error` queue cannot be easily retried at this time,
 so it is best to avoid this situation.

If it is problematic to register all endpoints with the bridge, the behavior can be disabled.

snippet: do-not-translate-reply-to-address-for-failed-messages

