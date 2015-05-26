---
title: Shared Hosting in Azure Cloud Services Sample
summary: 'Shared hosting in Azure Cloud Services.'
tags:
- Azure
- Cloud Services
- Hosting
- Shared hosting
- Hosting in Azure
related:
- nservicebus/azure/shared-hosting-in-azure-cloud-services
---

 1. Start [Azure Storage Emulator](http://azure.microsoft.com/en-us/documentation/articles/storage-use-emulator/)
 1. Rebuild solution (right click on `AzureSharedHosting` solution in Solution explorer, then `Rebuild Solution`)
 1. Run the solution 
 1. Inspect Azure Storage Emulator Tables for `MultiHostedEndpointsOutput` table and its content for something like the following:

| PartitionKey | RowKey | Timestamp | Message |
|:--|:--|:--|:--|
|Sender	|2015-05-25 14:58:33	|5/25/2015 8:58:33 PM	|Pinging Receiver |
|Receiver	|2015-05-25 14:58:34	|5/25/2015 8:58:34 PM	|Got Ping and will reply with Pong |
|Sender	|2015-05-25 14:58:35	|5/25/2015 8:58:35 PM	|Got Pong from Receiver |

WARNING: When running sample, do not stop execution from Visual Studio, but instead stop cloud service from Azure Compute Emulator UI. Do to an issue with Azure Compute Emulator not invoking OnStop method, multi hosted endpoints are not terminated.

NOTE: To inspect multi-host [role emulated file system](https://msdn.microsoft.com/en-us/library/azure/hh771389.aspx), navigate to c:\Users\<username>\AppData\Local\dftmp\Resources
 
## Code walk-through

This sample contains five projects: 

 * Shared - A class library containing shared code including the message definitions.
 * Sender - An NServiceBus endpoint responsible for sending `Ping` command designated for `Receiver` endpoint processing.
 * Receiver - An NServiceBus endpoint that receives `Ping` command and responds back to originator with `Pong` message.
 * HostWorker - Multi-host endpoint deployed as worker role.
 * HostCloudService - Azure Cloud Service project to define and execute cloud service.

### Sender project

Sender project defines message mapping to instruct NServiceBus to send `Ping` commands to `Receiver` endpoint.

<!-- import AzureMultiHost_MessageMapping -->

When Bus is started, ping command is sent and a custom verification log is written to Azure Storage Tables (see Shared project for details about verification log table).

<!-- import AzureMultiHost_SendPingCommand -->

Sender also defines a handler for messages of type `Pong` and writes into verification log when such arrives.

<!-- import AzureMultiHost_PongHandler -->

### Receiver project

Receiver project has a handler for `Ping` commands and it writes into verification log when such arrives and replies back to originator with `Pong` message.

<!-- import AzureMultiHost_PingHandler -->

### Shared project

Shared project defines all the messages used in the sample

<!-- import AzureMultiHost_PingMessage -->
<!-- import AzureMultiHost_PongMessage -->

### HostWorker project

HostWorker project is the multi-host project. To enable multi-hosting, endpoint is configured as a multi-host

<!-- import AzureSharedHosting_HostConfiguration -->

NOTE: multi-host project can be used as an another endpoint as well, for simplicity of this sample it was solely used as a host.

### HostCloudService project

HostCloudService project defines multi-host parameters for all environment (`Local` and `Cloud` in this sample)

<!-- import AzureSharedHosting_CloudServiceDefinition -->

Values provided to execute sample against local Azure Storage emulator

<!-- import AzureSharedHosting_CloudServiceConfiguration -->


