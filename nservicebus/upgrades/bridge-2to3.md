---
title:  Upgrade Version 2 to 3
reviewed: 2023-10-23
component: Bridge
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## New package for MSMQ support

In Version 3 support for bridging MSMQ endpoints are now provided by the new `NServiceBus.MessagingBridge.Msmq` package. Remove any references to `NServiceBus.Transports.Msmq` and update your bridge configuration from:

`var msmq = new BridgeTransport(new MsmqTransport());`

to:

`var msmq = new BridgeTransport(new MsmqBridgeTransport());`
