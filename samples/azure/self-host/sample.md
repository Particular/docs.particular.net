---
title: Self-Hosting in Azure Cloud Services
summary: Using the NServiceBus self-hosting capability to host an endpoint in an Azure instance.
component: Core
reviewed: 2019-07-02
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

Results are sorted by Timestamp


## Deploying endpoints

 1. Open a PowerShell console at the `self-host\CloudServicesHost_{version}` location. This location should contain a file called `PackageAndDeploy.ps1`.
 1. Execute the `PackageAndDeploy.ps1` PowerShell script to package and deploy multi-hosted endpoints to local emulator storage.


## Running a self-host in emulated Cloud Service

 1. Set `HostCloudService` to be the start-up project by right-clicking the project name in Visual Studio Solution Explorer, and selecting `Set as StartUp Project` option
 1. Run the solution
 1. Inspect Azure Storage Emulator tables for `SelfHostedEndpointsOutput` table and its content

NOTE: To inspect multi-host role emulated file system navigate to `C:\Users\%USERNAME%\AppData\Local\dftmp\Resources`

Azure Compute Emulator leaves any processes spawned at run time in memory. These can be killed by locating the `WaWorkerHost.exe` process and killing all child processes named `NServiceBus.Hosting.Azure.HostProcess.exe`. The number of those processes will be the same as the number of endpoints (i.e. one) x the number of times Cloud Service was executed.

Cloud Service emulator can also be stopped with the Compute Emulator UI. The Compute Emulator UI can be accessed via the try icon on the taskbar. Within Compute Emulator UI, under `Service Deployments` tree select a deployment, right-click, and select the `Remove` option. This will cleanly stop Cloud Service without leaving any processes in memory.


## Code walk-through

This sample contains five projects:

 * HostWorker - Self-host endpoint deployed as worker role.
 * HostCloudService - Azure Cloud Service project to define and execute cloud service.


### HostWorker project

HostWorker project uses the self-hosting capability to start an endpoint inside a worker role.

The snippet below illustrates how the `OnStart` method of `RoleEntryPoint` calls in a blocking way into an asynchronous start method.

snippet: AzureSelfHost_StartEndpoint

A critical error action needs to be defined to restart the host when a critical error is raised.

snippet: AzureSelfHost_CriticalError

To uniquely identify the host a custom name and display name must be provided.

snippet: AzureSelfHost_DisplayName

The connection string can be loaded using the `RoleEnvironment` as shown below.

snippet: AzureSelfHost_ConnectionString

### HostCloudService project

The HostCloudService project defines self-host parameters for all environments (`Local` and `Cloud` in this sample)

snippet: AzureSelfHosting_CloudServiceDefinition

Values provided to execute the sample against local Azure Storage emulator

snippet: AzureSelfHosting_CloudServiceConfiguration