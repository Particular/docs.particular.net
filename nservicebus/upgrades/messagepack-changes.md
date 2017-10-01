---
title: MessagePack NuGet changes
summary: Changes to MessagePack to support both the MessagePack and MsgPack.Cli NuGet packages.
reviewed: 2017-09-28
component: MessagePack
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
related:
 - nservicebus/serialization/messagepack
 - nservicebus/serialization/msgpack
---

The [NServiceBus.MessagePack project](https://github.com/SimonCropp/NServiceBus.MessagePack) previously leveraged the [MsgPack-CLI project](https://github.com/msgpack/msgpack-cli) to enable [MessagePack](http://msgpack.org/) serialization. This was problematic since there are two different implementations of MessagePack that target .NET

 * [MsgPack-CLI project](https://github.com/msgpack/msgpack-cli)
 * [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp)

To support both, MsgPack-CLI support has been moved to [NServiceBus.MsgPack](https://github.com/SimonCropp/NServiceBus.MsgPack). [NServiceBus.MessagePack](https://github.com/SimonCropp/NServiceBus.MessagePack) has been changed to support MessagePack-CSharp.

To continue using MessagePack via the MsgPack-CLI implementation uninstall the [NServiceBus.MessagePack Package](https://www.nuget.org/packages/NServiceBus.MessagePack/) and install the [NServiceBus.MsgPack Package](https://www.nuget.org/packages/NServiceBus.MsgPack/).

Otherwise update to the latest [NServiceBus.MessagePack Package](https://www.nuget.org/packages/NServiceBus.MessagePack/) and begin using [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp)