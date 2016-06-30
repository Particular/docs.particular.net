---
title: Azure Storage Persistence upgrade Version 6.2.4 to 6.2.5
summary: Instructions on how to patch Azure Storage Persistence when orphan saga index records appear.
reviewed: 2016-06-30
tags:
 - upgrade
related:
 - nservicebus/sagas
 - nservicebus/azure-storage-persistence
---


## Summary

This document explains how to upgrade and patch a system for [Azure Storage Persistence bug #74](https://github.com/Particular/NServiceBus.Persistence.AzureStorage/issues/74) using NServiceBus.Azure hotfix release 6.2.5.

WARNING: When upgrading to NServiceBus.Persistence.AzureStorage Version 1 and above, the following upgrade will need to be performed prior to beginning any other upgrade steps.


### How to know if a system may be affected

This bug will affect any system that has ever used sagas and NServiceBus.Azure v6.2.4 or lower.


## Patch Requirements

To deploy this fix throughout a system, all endpoints will need to be upgraded and saga data that has been stored by the Azure Storage persister will need to be patched.


### Upgrading endpoints

All endpoints using NServiceBus.Azure will need to be upgraded to version 6.2.5 or higher.


### Patching data

Saga data stored in Azure will need to be patched using the `IndexPruner` utility which can be downloaded from [https://github.com/Particular/IssueDetection/releases/tag/nsb.azure.284](https://github.com/Particular/IssueDetection/releases/tag/nsb.azure.284)


## Patch steps

 1. Download the index pruning tool from [https://github.com/Particular/IssueDetection/releases/tag/nsb.azure.284](https://github.com/Particular/IssueDetection/releases/tag/nsb.azure.284) and put it on a computer that has internet access as well as the .NET Framework 4.5.2 installed.
 1. Optionally, add an Azure Storage connection string to the `IndexPruner.exe.config` file uses `name="sagas"`. If the connection string is not added to `IndexPruner.exe.config` it will have to be provided as a commandline parameter in step 4 below. Config file example:
	```xml
	<configuration>
		<connectionStrings>
			<add name="sagas" connectionStrings="--anAzureStorageConnectionString--"/>
		</connectionStrings>
	</configuration>
	```
 1. Copy all endpoint dlls to the same folder as the index pruning tool. These files will be scanned to find all implementations of `IContainSagaData` which will indicate the sagas that need to be pruned in Azure Storage.
 1. Open a commandline and run the following command: `IndexPruner.exe`. If you did not put the Azure connection string in the `IndexPruner.exe.config` file in step 2 you will need to run the command as `IndexPruner.exe &ltconnectionstringvalue&gt`. While running, the tool will output details of the actions that it is taking.
 1. Update NServiceBus.Azure dependency to version 6.2.5 or higher in all endpoints that use it and release the updated endpoints.
