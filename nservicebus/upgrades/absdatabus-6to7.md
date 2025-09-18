---
title: Azure Blob Storage Databus Upgrade Version 6 to 7
summary: Instructions on how to upgrade Azure Blob Storage Databus from version 6 to 7.
reviewed: 2025-09-15
component: ABSDataBus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
 - 10
---

## Changes due to ClaimCheck been released as a separate package

In order to incorporate the changes related to the creation of the new NServiceBus.ClaimCheck package, the classes and interface that mention `DataBus` have been obsoleted in favor of their new `ClaimCheck` counterparts.

Their usage and APIs remains the same.


|Obsolete|Replace with|
|---|---|
|class AzureDataBus| class AzureClaimCheck|
|class class ConfigureAzureDataBus| class ConfigureAzureClaimCheck|
|interface NServiceBus.DataBus.AzureBlobStorage.IProvideBlobServiceClient| interface NServiceBus.ClaimCheck.AzureBlobStorage.IProvideBlobServiceClient|


For more details about the migration to the new package, visit the [ClaimCheck section of the NServiceBus upgrade guide from 9 to 10](/nservicebus/upgrades/9to10/#databus-feature-moved-to-separate-nservicebus-claimcheck-package).

