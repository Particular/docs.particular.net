---
title:  Messaging Bridge Upgrade Version 2 to 3
reviewed: 2024-09-12
component: Bridge
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
 - 9
---

## New package for MSMQ support

In version 3, support for bridging MSMQ endpoints is now provided by the new `NServiceBus.MessagingBridge.Msmq` package. Remove any references to `NServiceBus.Transports.Msmq` and update the bridge configuration from:

`var msmq = new BridgeTransport(new MsmqTransport());`

to:

`var msmq = new BridgeTransport(new MsmqBridgeTransport());`
