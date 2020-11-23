---
title: Azure Storage Persistence upgrade Version 6.2.4 to 6.2.5
summary: Instructions on how to patch Azure Storage Persistence when orphan saga index records appear.
reviewed: 2020-09-02
component: ASP
related:
 - nservicebus/sagas
 - persistence/azure-table
redirects:
 - nservicebus/upgrades/asp-saga-pruning
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
---

This document explains how to upgrade and patch a system for [Azure Storage Persistence bug #74](https://github.com/Particular/NServiceBus.Persistence.AzureStorage/issues/74) using NServiceBus.Azure hotfix release 6.2.5.

WARNING: When upgrading to NServiceBus.Persistence.AzureStorage version 1 and above, the following upgrade will need to be performed prior to beginning any other upgrade steps.

WARNING: The [saga de-duplication patch](/persistence/upgrades/asp-saga-deduplication.md) process must be completed at least once prior to proceeding with this update.

## How to know if a system may be affected

This bug will affect any system that has ever used sagas and NServiceBus.Azure versions 6.2.4 or below.


## Patch requirements

To deploy this fix throughout a system, all endpoints must be upgraded and saga data that has been stored by the Azure Storage persister must be patched.


### Upgrading endpoints

All endpoints using NServiceBus.Azure must be upgraded to version 6.2.5 or above.


### Patching data

Saga data stored in Azure must be patched using the `IndexPruner` utility which can be downloaded from [https://github.com/Particular/IssueDetection/releases/tag/nsb.azure.284](https://github.com/Particular/IssueDetection/releases/tag/nsb.azure.284). This utility will remove all orphaned secondary indexes from the Azure Storage Tables.


## Patch steps

 1. Download the index pruning tool from [https://github.com/Particular/IssueDetection/releases/tag/nsb.azure.284](https://github.com/Particular/IssueDetection/releases/tag/nsb.azure.284) to a computer that has the .NET Framework 4.5.2 installed and also has internet access.
 1. The `IndexPruner` utility requires an Azure Storage connection string. The connection string can be added to the `IndexPruner.exe.config` file with the `name="sagas"` value or provided as a command line parameters (shown in step 4).
  ```xml
  <configuration>
      <connectionStrings>
          <add name="sagas"
               connectionStrings="--anAzureStorageConnectionString--"/>
      </connectionStrings>
  </configuration>
  ```
 1. Copy the endpoint dll along with the assemblies that contain saga type definitions to the same directory as the index pruning tool. If multiple endpoints require this patch, it is ok to add the assemblies for all the endpoints to the tool. This saves time from having to run the tool for each endpoint. These assemblies will be scanned to find all implementations of `IContainSagaData` which will indicate the sagas that need to be pruned in Azure Storage.
 1. Open a command line and run the following command: `IndexPruner.exe`. If the Azure connection string was not added to the `IndexPruner.exe.config` file in step 2, the command needed to run the `IndexPruner` will be `IndexPruner.exe <connectionstringvalue>`. While running, the `IndexPruner` will output details of the actions that it is taking to the command window.
 1. Update the NServiceBus.Azure dependency to version 6.2.5 or higher in all endpoints that use it and release the updated endpoints.
