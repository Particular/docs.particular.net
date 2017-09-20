---
title: Self Hosting in Azure Cloud Services
summary: Uses NServiceBus self-hosting capability to host an endpoint in an Azure instance.
component: Core
tags:
- Azure
- Hosting
related:
- nservicebus/hosting/cloud-services-host/shared-hosting
---

## Running in development mode

 1. Start [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator)
 1. Run the solution
 1. Inspect Azure Storage Emulator Tables for `SelfHostedEndpointsOutput` table and its content for something like the following:

| PartitionKey | RowKey | Timestamp | Message |
|:--|:--|:--|:--|
|MyMessageHandler	|2017-09-19 10:19:08	|2017-09-19T08:19:27.691Z	|Got MyMessage. |

Results sorted by Timestamp


## Deploying endpoints

 1. Open PowerShell console at the `self-host\CloudServicesHost_{version}` location. This location should contain `PackageAndDeploy.ps1`.
 1. Execute `PackageAndDeploy.ps1` PowerShell script to package and deploy multi-hosted endpoints to local emulator storage


## Running self-host in emulated Cloud Service

 1. Set `HostCloudService` to be the start-up project by right clicking the project name in Visual Studio Solution Explorer, and selecting `Set as StartUp Project` option
 1. Run the solution
 1. Inspect Azure Storage Emulator Tables for `SelfHostedEndpointsOutput` table and its content

NOTE: To inspect multi-host role emulated file system navigate to `C:\Users\%USERNAME%\AppData\Local\dftmp\Resources`

Azure Compute Emulator leaves any processes spawned at run time in memory. Kill those once done with emulated Cloud Service execution by locating `WaWorkerHost.exe` process and killing all child processes named `NServiceBus.Hosting.Azure.HostProcess.exe`. Number of those processes will be as number of endpoints (1) X number of times Cloud Service was executed.

Cloud Service emulator can also be stopped from Compute Emulator UI. Compute Emulator UI can be accessed via try icon on the taskbar. Within Compute Emulator UI, under `Service Deployments` tree select a deployment, right click and select `Remove` option. This will cleanly stop Cloud Service without leaving any processes in memory.


## Code walk-through

This sample contains five projects:

 * HostWorker - Self-host endpoint deployed as worker role.
 * HostCloudService - Azure Cloud Service project to define and execute cloud service.


### HostWorker project

HostWorker project uses the self-hosting capability to start an endpoint inside a worker role.

The snippet below illustrates how the `OnStart` method of the `RoleEntryPoint` calls in a blocking way into an asynchronous start method.

snippet: AzureSelfHost_StartEndpoint

A critical error action needs to be defined to restart the host when a critical error is raised.

snippet: AzureSelfHost_CriticalError

To uniquely identify the host a custom name and display name need to be provided.

snippet: AzureSelfHost_DisplayName

Connection string can be loaded by leveraging the `RoleEnvironment` as shown below.

snippet: AzureSelfHost_ConnectionString

### HostCloudService project

HostCloudService project defines self-host parameters for all environment (`Local` and `Cloud` in this sample)

snippet: AzureSelfHosting_CloudServiceDefinition

Values provided to execute sample against local Azure Storage emulator

snippet: AzureSelfdHosting_CloudServiceConfiguration
