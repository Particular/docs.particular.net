---
title: Upgrade Version 8 to 9
summary: Instructions on how to upgrade NServiceBus from version 8 to version 9.
reviewed: 2022-04-22
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
 - 9
---

## SendOptions changes

In NServiceBus version 8 and earlier, the indication that a message was marked for [immediate dispatch](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately) was provided by `SendOptions.RequiredImmediateDispatch()`. In version 9, this method has been renamed to `SendOptions.IsImmediateDispatchSet()`.

## DataBus interface changes

Starting with NServiceBus version 8, it is mandatory to provide a serializer to the data bus configuration API. This is now enforced in NServiceBus version 9. The `BinaryFormatterDataBusSerializer` has been removed and `SystemJsonDataBusSerializer` is the default option that's built-in.
