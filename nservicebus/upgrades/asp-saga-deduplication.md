---
title: Patch Azure Storage Persistence Sagas
summary: Instructions on how to patch Azure Storage Persistence when saga duplication occurs.
tags:
 - upgrade
related:
 - nservicebus/sagas
 - nservicebus/azure/azure-storage-persistence
---

## Summary

This document explains how to upgrade and patch a system for [Azure Storage Persistence bug #634](https://github.com/Particular/PlatformDevelopment/issues/634) using the NServiceBus Azure hotfix release xxxxxxx.

### How to know if a system is affected
This bug will affect a system if both of the following conditions are met:
- more than one IAmStartedBy<T> for a given saga
- concurrency is set to > 1

### Symptoms
The following are three of the symptoms that a system may exhibit as result of this bug.
- sending more than one message when only one should be sent
- sending no messages when one should be sent
- creation of multiple instances of the same saga

## Patch Requirements
To deploy this fix throughout a system endpoints will need to be upgraded and saga data that has been stored by the Azure Storage persister will need to be patched.

### Upgrading endpoints

All endpoints using NServiceBus Azure will need to be upgraded to version 6.2.4 or higher.

### Patching data
Saga data stored in Azure will need to be patched using the `NServiceBus.AzureStoragePersistence.SagaDeduplicator' utility which can be downloaded from xxxxxxxxx.


## Patch steps

Steps:
1. Run de-duplication utility
2. Update NServiceBus.Azure dependency in all endpoints that use it
3. Run de-duplication utility


NOTE: Minimizing the time between steps one and two will minimize the work required later in the process.

### Upgrade endpoint configurations
Update references to NServiceBus.Azure to version 6.2.4 or later.

### Running the de-duplication utility
1. Download the de-duplication tool from xxxxxxxxx
2. Run the de-duplication tool from any computer that has both an internet connection and the .NET Framework v4.5.2 installed
3. Open a commandline and run the following command: `NServiceBus.AzureStoragePersistence.SagaDeduplicator.exe directory=<directory> sagaTypeName=<sagaTypeName> sagaProperty=<sagaProperty> operation=Download connectionString=<connectionString>`
	- **directory**: This should be the full path of the working directory that the de-duplication tool will use. The tool will need to have read, write and delete permissions on the folder selected. If there are any spaces in the directory path this value should be enclosed in double quotes. Example: `directory="C:\my working folder\deduplication"`
	- **sagaTypeName**:
	- **sagaProperty**:
	- **operation**: "Download"
	- **connectionString**: This parameter is optional and there are two ways to provide the Azure connection string.
		1. As a commandline parameter. Example: `connectionString=DefaultEndpointsProtocol=https;AccountName=anAzureStorageAccount;AccountKey=FuiDeGgJoHackI4h1d44LFK0ceqfkxDMO79In+ofWG42rw9kNDyQuErNmJ8WyJ7GLa+xy5NVaN21lOMZUevaQw==;BlobEndpoint=https://anAzureStorageAccount.blob.core.windows.net/;TableEndpoint=https://anAzureStorageAccount.table.core.windows.net/;QueueEndpoint=https://anAzureStorageAccount.queue.core.windows.net/;FileEndpoint=https://anAzureStorageAccount.file.core.windows.net/`
		2. As a connectionStrings entry in the `NServiceBus.AzureStoragePersistence.SagaDeduplicator.exe.config`. Example:  
```		
<configuration>  
  <connectionStrings>  
		<add name="sagas" connectionStrings="--anAzureStorageConnectionString--"/>  
	</connectionStrings> 
</configuration>
```
4. what about conflicts?
5. Run the following command: `NServiceBus.AzureStoragePersistence.SagaDeduplicator.exe directory=<directory> sagaTypeName=<sagaTypeName> sagaProperty=<sagaProperty> operation=Upload connectionString=<connectionString>`
	- All of the commandline parameters, with the exception of `operation=Upload`, should be exactly the same as they were in step #3.


## After the patch process
Once the patch process has been completed there will be exceptions thrown, and logged, when duplicates are found in the course of normal message processing. The exception that is logged will be `DuplicateSagaFoundException` and will have the following message structure:

	Sagas of type {sagaType.Name} with the following identifiers '<comma separated list of message identifiers>' are considered duplicates because of the violation of the Unique property {propertyName}.

When this happens the involved messages will be sent to the error queue. Once in the error queue those messages will need to be re-queued so that they can be reprocessed. To re-queue messages follow the instructions for [Errors and Retries using Service Insight](/serviceinsight/#errors-and-retries).