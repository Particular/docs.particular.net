---
title: Shared Hosting in Azure Cloud Services
summary: Uses the NServiceBus Hosting Azure HostProcess to achieve shared hosting of multiple NServiceBus endpoints in an Azure instance.
component: CloudServicesHost
reviewed: 2017-06-12
tags:
- Azure
- Hosting
related:
- nservicebus/hosting/cloud-services-host/shared-hosting
---


## Running in development mode

 1. Start [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator)
 1. Run the solution
 1. Inspect Azure Storage Emulator Tables for `MultiHostedEndpointsOutput` table and its content for something like the following:

| PartitionKey | RowKey | Timestamp | Message |
|:--|:--|:--|:--|
|Sender	|2015-05-25 14:58:33	|2015-05-25 8:58:33 PM	|Pinging Receiver |
|Receiver	|2015-05-25 14:58:34	|2015-05-25 8:58:34 PM	|Got Ping and will reply with Pong |
|Sender	|2015-05-25 14:58:35	|2015-05-25 8:58:35 PM	|Got Pong from Receiver |

Results sorted by Timestamp


## Deploying endpoints

 1. Open PowerShell console at the `shared-host\CloudServicesHost_{version}` location. This location should contain `PackageAndDeploy.ps1`.
 1. Execute `PackageAndDeploy.ps1` PowerShell script to package and deploy multi-hosted endpoints to local emulator storage


## Running multi-host in emulated Cloud Service

 1. Set `HostCloudService` to be the start-up project by right clicking the project name in Visual Studio Solution Explorer, and selecting `Set as StartUp Project` option
 1. Run the solution
 1. Inspect Azure Storage Emulator Tables for `MultiHostedEndpointsOutput` table and its content

NOTE: To inspect multi-host role emulated file system navigate to `C:\Users\%USERNAME%\AppData\Local\dftmp\Resources`

Azure Compute Emulator leaves any processes spawned at run time in memory. Kill those once done with emulated Cloud Service execution by locating `WaWorkerHost.exe` process and killing all child processes named `NServiceBus.Hosting.Azure.HostProcess.exe`. Number of those processes will be as number of endpoints (2) X number of times Cloud Service was executed.

Cloud Service emulator can also be stopped from Compute Emulator UI. Compute Emulator UI can be accessed via try icon on the taskbar. Within Compute Emulator UI, under `Service Deployments` tree select a deployment, right click and select `Remove` option. This will cleanly stop Cloud Service without leaving any processes in memory.


## Code walk-through

This sample contains five projects:

 * Shared - A class library containing shared code including the message definitions.
 * Sender - An NServiceBus endpoint responsible for sending `Ping` command designated for `Receiver` endpoint processing.
 * Receiver - An NServiceBus endpoint that receives `Ping` command and responds back to originator with `Pong` message.
 * HostWorker - Multi-host endpoint deployed as worker role.
 * HostCloudService - Azure Cloud Service project to define and execute cloud service.


### Sender project

Sender project defines message mapping to instruct NServiceBus to send `Ping` commands to `Receiver` endpoint.

snippet: AzureMultiHost_MessageMapping

When Bus is started, ping command is sent and a custom verification log is written to Azure Storage Tables (see Shared project for details about verification log table).

snippet: AzureMultiHost_SendPingCommand

Sender also defines a handler for messages of type `Pong` and writes into verification log when such arrives.

snippet: AzureMultiHost_PongHandler


### Receiver project

Receiver project has a handler for `Ping` commands and it writes into verification log when such arrives and replies back to originator with `Pong` message.

snippet: AzureMultiHost_PingHandler


### Shared project

Shared project defines all the messages used in the sample

snippet: AzureMultiHost_PingMessage
snippet: AzureMultiHost_PongMessage


### HostWorker project

HostWorker project is the multi-host project. To enable multi-hosting, endpoint is configured as a multi-host

snippet: AzureSharedHosting_HostConfiguration

NOTE: Multi-host project is used solely as a host for other endpoints


### HostCloudService project

HostCloudService project defines multi-host parameters for all environment (`Local` and `Cloud` in this sample)

snippet: AzureSharedHosting_CloudServiceDefinition

Values provided to execute sample against local Azure Storage emulator

snippet: AzureSharedHosting_CloudServiceConfiguration
