---
title: Azure Storage Persistence upgrade version 6.2.3 to 6.2.4
summary: Instructions on how to patch Azure Storage Persistence when saga duplication occurs.
reviewed: 2019-11-26
component: ASP
related:
 - nservicebus/sagas
 - persistence/azure-table
redirects:
 - nservicebus/upgrades/asp-saga-deduplication
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
---

This article explains how to upgrade and patch a system for [Azure Storage Persistence bug #26](https://github.com/Particular/NServiceBus.Persistence.AzureStorage/issues/26) using the NServiceBus.Azure hotfix release 6.2.4.

WARNING: When upgrading to NServiceBus.Persistence.AzureStorage version 1 and above, the following upgrade must be performed prior any other upgrade steps.


## How to know if a system may be affected

This bug will affect a system only if the following conditions are met on the same endpoint:

 * NServiceBus version 5 or below.
 * NServiceBus.Azure version 6.2.3 or below.
 * Azure Storage Persistence is used.
 * the endpoint contains a saga which has more than one `IAmStartedByMessages<T>`.
 * endpoint concurrency is set to a value **greater than** 1.


## Patch requirements

To deploy this fix throughout a system, all endpoints must be upgraded and saga data that has been stored by the Azure Storage persister will need to be patched.


### Upgrading endpoints

All endpoints using NServiceBus.Azure will need to be upgraded to version 6.2.4 or above.


### Patching data

Saga data stored in Azure must be patched using the `NServiceBus.AzureStoragePersistence.SagaDeduplicator` utility which can be downloaded from [https://github.com/Particular/IssueDetection/releases/tag/nsb.asp.26](https://github.com/Particular/IssueDetection/releases/tag/nsb.asp.26).


## Patch steps

 1. Download the de-duplication tool from [https://github.com/Particular/IssueDetection/releases/tag/nsb.asp.26](https://github.com/Particular/IssueDetection/releases/tag/nsb.asp.26) and put it on a computer that has internet access as well as the .NET Framework 4.5.2 installed.
 1. Add an Azure Storage connection string to the `NServiceBus.AzureStoragePersistence.SagaDeduplicator.exe.config` file. For example:
  ```xml
  <configuration>
      <connectionStrings>
          <add name="sagas"
               connectionStrings="--anAzureStorageConnectionString--"/>
     </connectionStrings>
  </configuration>
  ```
 1. Copy the endpoint dlls to the same directory as the de-duplication tool. These files will be scanned to find all implementations of `IContainSagaData` which will indicate the sagas that need to be verified in Azure Storage.
 1. Run the de-duplication utility (refer to the [Running the de-duplication utility](#patch-steps-running-the-de-duplication-utility) section for more details).
 1. All class names returned by the de-duplication tool in the previous step will need to add the `[Unique]` attribute to one property. `IContainSagaData` classes without a property decorated by the `[Unique]` attribute will cause their sagas to throw exceptions post-upgrade.
 1. Update NServiceBus.Azure to version 6.2.4 or above in all endpoints that use it and release the updated endpoints.
 1. Run the de-duplication utility again (see below for details). This will fix problem saga data or list conflicts that were introduced to the data store while steps #5 and #6 were being performed.
 1. If the de-duplication utility has output any classes that require the identification of a correlation property, then return to step #5 above and address those classes.


### Running the de-duplication utility


#### 1. Run the SagaDeduplicator utility

Open a command line and run the following command: `NServiceBus.AzureStoragePersistence.SagaDeduplicator.exe directory=<directory> operation=Download` where `<directory>` is the full path of the working directory that the de-duplication tool will use for storing conflicting sagas payloads. The tool must have read, write and delete permissions on the directory selected. If there are any spaces in the directory path this value should be enclosed in double quotes. If the tool was run previously, ensure that the directory indicated is empty. For example: `directory="C:\my working directory\deduplication"`.


#### 2. Utility output

The utility will output a list of saga data classes that it found while scanning the assemblies provided. The list of classes is split into two categories: those classes that have a correlation property and those that do not.

```dos
$ NServiceBus.AzureStoragePersistence.SagaDeduplicator.exe directory=saga operation=Download

Following saga types have correlation properties

Following saga types have NO correlation property marked with [Unique] and won't be searched for duplicates.
    * OrderSagaState
```

The saga classes that do have a correlation property will have their data de-duplicated. The classes that do not have a correlation property will need to have the `[Unique]` attribute added later steps. Make note of the classes in this list for later use.


#### 3. Unresolved conflicts

The de-duplication utility will fix the data for all saga instances that it can. However, there can be situations where the utility was unable to resolve the correct saga instance. When this happens, the de-duplication tool will download conflicting sagas to the working directory that was provided as a command line parameter. Every set of conflicting sagas is downloaded to a separate directory containing files named with conflicting sagas identifiers. For example, running the utility with `directory=data` against an assembly that has a `TwoInstanceSagaState` class that implements `IContainSagaData` will result in a following structure:

```
data
└───TwoInstanceSagaState
    └───d10c2f15-06d2-1370-e0ee-781710b5d598
        └───0e36dc9a-eec0-455c-a4d3-b8b275711d15
        └───8ee2f4b2-eaf2-4d12-a87e-a5e000aaa815
```

Where the `0e36dc9a-eec0-455c-a4d3-b8b275711d15` and `8ee2f4b2-eaf2-4d12-a87e-a5e000aaa815` directories contain the JSON payload of each of the conflicting sagas. In addition to the saga properties, the JSON payload contains a property, `"$Choose_this_saga"`, to indicate which of the conflicting sagas is to be used as the selected saga.

```json
{
  "OrderId": "8ca3f5a2-009f-4796-9727-d8493e47288f",
  "Id": "0e36dc9a-eec0-455c-a4d3-b8b275711d15",
  "Originator": "Originator",
  "Name": "Test",
  "OriginalMessageId": "fe0414cd-d440-494a-b21a-a5e000aaa68e",
  "$ETag": "W/\"datetime'2016-04-06T08%3A21%3A23.7356692Z'\"",
  "$Choose_this_saga": false
}
```

Initially, its value is `"$Choose_this_saga": false`. For each set of conflicting sagas, one saga should be updated accordingly and marked chosen by setting the `$Choose_this_saga` property to `true`.

```json
{
  "OrderId": "8ca3f5a2-009f-4796-9727-d8493e47288f",
  "Id": "0e36dc9a-eec0-455c-a4d3-b8b275711d15",
  "Originator": "Originator",
  "Name": "------------------------This is updated name------------------",
  "OriginalMessageId": "fe0414cd-d440-494a-b21a-a5e000aaa68e",
  "$ETag": "W/\"datetime'2016-04-06T08%3A21%3A23.7356692Z'\"",
  "$Choose_this_saga": true
}
```

This saga selection will need to be performed on all sagas that are downloaded to the file system.


#### 4. Update the Azure Storage

Once all conflicting sagas have been resolved, run the following command: `NServiceBus.AzureStoragePersistence.SagaDeduplicator.exe directory=<directory> operation=Upload`. This step will update the Azure Storage to contain the conflicted sagas that were marked `"$Choose_this_saga": true` in step #3.

 * All of the command line parameters, with the exception of `operation=Upload`, should be the same as they were in step #1.


## After the patch process

Once the patch process has been completed there will be exceptions thrown, and logged, when duplicates are found in the course of normal message processing. The exception that is logged will be `DuplicateSagaFoundException` and will have the following message structure:

```
Sagas of type {sagaType.Name} with the following identifiers '<comma separated list of message identifiers>' are considered duplicates because of the violation of the Unique property {propertyName}.
```

When this happens the involved messages will be sent to the error queue. There are two ways to re-queue these messages.

 * If ServiceControl and ServicePulse are running, follow the instructions for [Failed Message Retry using ServicePulse](/servicepulse/intro-failed-message-retries.md) to requeue the messages.
 * To re-queue messages without ServicePulse and/or ServiceControl, the messages can be manually moved from the error queue to the appropriate processing queue. The way this is done will vary depending on the transport being used.
