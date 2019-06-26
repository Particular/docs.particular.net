---
title: NServiceBus.MessagePack
summary: Changes to MessagePack to support both the MessagePack and MsgPack.Cli NuGet packages.
reviewed: 2019-06-26
component: MessagePack
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
related:
 - nservicebus/serialization/messagepack
 - nservicebus/serialization/msgpack
---

[NServiceBus.MessagePack](https://nuget.org/packages/NServiceBus.MessagePack/) previously used [MsgPack.Cli](https://www.nuget.org/packages/MsgPack.Cli/). Starting with Version 2, NServiceBus.MessagePack uses the [MessagePack NuGet package](https://www.nuget.org/packages/MessagePack/).

To continue using the MsgPack.Cli implementation, uninstall NServiceBus.MessagePack and install [NServiceBus.MsgPack](https://www.nuget.org/packages/NServiceBus.MsgPack/).
