---
title: Azure Blob Storage Data Bus Upgrade Version 6 to 7
summary: Instructions on how to upgrade Azure Blob Storage Data Bus from version 6 to 7.
reviewed: 2025-09-15
component: ABSDataBus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
 - 10
---

## Types renamed because of new NServiceBus.ClaimCheck package

In order to incorporate the changes related to the creation of the new [NServiceBus.ClaimCheck](https://www.nuget.org/packages/NServiceBus.ClaimCheck/) package, the types that mention `DataBus` have been renamed to use term `ClaimCheck` instead.

The table below shows the mapping from the DataBus types to their ClaimCheck equivalents.

|DataBus|ClaimCheck|
|---|---|
|`NServiceBus.AzureDataBus`|`NServiceBus.AzureClaimCheck`|
|`NServiceBus.ConfigureAzureDataBus`| `NServiceBus.ConfigureAzureClaimCheck`|
|`NServiceBus.DataBus.AzureBlobStorage.IProvideBlobServiceClient`| `NServiceBus.ClaimCheck.AzureBlobStorage.IProvideBlobServiceClient`|

For more details about the migration to the new package, visit the [ClaimCheck section of the NServiceBus upgrade guide from 9 to 10](/nservicebus/upgrades/9to10/#databus-feature-moved-to-separate-nservicebus-claimcheck-package).