---
title: Azure Service Bus Transport (Legacy) Upgrade Version 9 to 10
summary: Tips for upgrading Azure Service Bus transport from version 9 to 10.
reviewed: 2020-03-02
component: ASB
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 ---


## .NET Framework 4.6.2.

Version 10 of the Azure Service Bus transport depends on version 6.0.0 of the WindowsAzure.ServiceBus package, which in turn requires at least .NET Framework 4.6.2.

Follow the [official guidance](/transports/upgrades/asb-10toasbs-1.md#api-differences-azure-cli-options) to determine which .NET Framework versions are installed, and install the required version if needed from https://www.microsoft.com/en-us/download/details.aspx?id=53345.