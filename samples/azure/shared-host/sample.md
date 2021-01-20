---
title: Shared Hosting in Azure Cloud Services
summary: Uses the NServiceBus Hosting Azure HostProcess to achieve shared hosting of multiple NServiceBus endpoints in an Azure instance.
component: CloudServicesHost
reviewed: 2021-01-11
related:
- nservicebus/hosting/cloud-services-host/shared-hosting
---


## Running in development mode

 1. Start [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator)
 1. Run the solution
 1. Inspect Azure Storage Emulator `MultiHostedEndpointsOutput` table for content similar to:

| PartitionKey | RowKey | Timestamp | Message |
|:--|:--|:--|:--|
|Sender	|2015-05-25 14:58:33	|2015-05-25 16:58:33	|Pinging Receiver |
|Receiver	|2015-05-25 14:58:34	|2015-05-25 16:58:34	|Got Ping and will reply with Pong |
|Sender	|2015-05-25 14:58:35	|2015-05-25 16:58:35	|Got Pong from Receiver |

Results sorted by Timestamp


## Deploying endpoints

 1. Open PowerShell console at the `shared-host\CloudServicesHost_{version}` location. This location should contain `PackageAndDeploy.ps1`.
 1. Execute `PackageAndDeploy.ps1` PowerShell script to package and deploy multi-hosted endpoints to local emulator storage


## Running multi-host in emulated Cloud Service

 1. Set `HostCloudService` to be the start-up project by right clicking the project name in Visual Studio Solution Explorer, and selecting `Set as StartUp Project` option
 1. Run the solution
 1. Inspect Azure Storage Emulator Tables for `MultiHostedEndpointsOutput` table and its content

NOTE: To inspect multi-host role emulated file system navigate to `C:\Users\%USERNAME%\AppData\Local\dftmp\Resources`

Azure Compute Emulator leaves any processes spawned at run time in memory. Kill those once done with emulated Cloud Service execution by locating `WaWorkerHost.exe` process and killing all child processes named `NServiceBus.Hosting.Azure.HostProcess.exe`. Number of those processes will equal the number of endpoints (which in this case equals 2) multiplied by the number of times Cloud Service was executed.

Cloud Service emulator can also be stopped from Compute Emulator UI. Compute Emulator UI can be accessed via try icon on the taskbar. Within Compute Emulator UI, under `Service Deployments` tree select a deployment, right click and select `Remove` option. This will stop Cloud Service without leaving any processes in memory.


## Code walk-through

This sample contains five projects:

 * Shared - A class library containing shared code including the message definitions.
 * Sender - An NServiceBus endpoint responsible for sending `Ping` command to the `Receiver` endpoint processing.
 * Receiver - An NServiceBus endpoint that receives `Ping` command and responds back to originator with `Pong` message.
 * HostWorker - Multi-host endpoint deployed as worker role.
 * HostCloudService - Azure Cloud Service project to define and execute cloud service.


### Sender project

Sender project defines message mapping instructing NServiceBus to send `Ping` commands to the `Receiver` endpoint.

snippet: AzureMultiHost_MessageMapping

When the endpoint is started, a `Ping` command is sent and a log entry is written to Azure Storage Tables (see the Shared project for details on the log generation).

snippet: AzureMultiHost_SendPingCommand

Sender defines a handler for messages of type `Pong` and writes to the log when a message of this type arrives.

snippet: AzureMultiHost_PongHandler


### Receiver project

Receiver project has a handler for `Ping` commands and it writes to the log when such a message arrives. It also replies to the originator with the `Pong` message.

snippet: AzureMultiHost_PingHandler


### Shared project

Shared project defines all the messages used in the sample

snippet: AzureMultiHost_PingMessage
snippet: AzureMultiHost_PongMessage


### HostWorker project

HostWorker project is the multi-host project. To enable multi-hosting, endpoint is configured as a multi-host

snippet: AzureSharedHosting_HostConfiguration

NOTE: Multi-host project is used only as a host for other endpoints and contains no NServiceBus related logic


### HostCloudService project

HostCloudService project defines multi-host parameters for all environment (`Local` and `Cloud` in this sample)

snippet: AzureSharedHosting_CloudServiceDefinition

Values provided to execute sample against local Azure Storage emulator

snippet: AzureSharedHosting_CloudServiceConfiguration
