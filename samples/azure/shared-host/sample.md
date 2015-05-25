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

## Prerequisites

1. Azure Cloud Services and NServiceBus Azure Storage Queues transport understanding
1. Azure Storage Emulator - know how to run/stop

---
TEMP: steps 
---

- Create cloud service with 1 worker role, host, and mark it as `AsA_Host`
- Defined for .csdef dynamichost required properties and local storage to pull down zips
- Created `Sender` project with reference to special host `NServiceBus.Hosting.Azure.HostProcess`
- Set on `Sender` and `Receiver` external program for debugging to be `NServiceBus.Hosting.Azure.HostProcess.exe` from each project`s debug folder respectively (optional to avoid compute emulator)
- Map `Ping` command to go to `Receiver` endpoint
- Zipped using C# inline msbuild task
- Uploaded to azure emulator with AzCopy utility http://azure.microsoft.com/en-us/documentation/articles/storage-use-azcopy/

-=-=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
 1. Start [Azure Storage Emulator](http://azure.microsoft.com/en-us/documentation/articles/storage-use-emulator/)
 1. Rebuild solution!!!
 1. Run the solution. 
 1. Inspect Azure Storage emulator Tables for `MultiHostedEndpointsOutput` table for similar entries.

| PartitionKey | RowKey | Timestamp | Message |
|:--|:--|:--|:--|
|Sender	|2015-05-25 14:58:33	|5/25/2015 8:58:33 PM	|Pinging Receiver |
|Receiver	|2015-05-25 14:58:34	|5/25/2015 8:58:34 PM	|Got Ping and will reply with Pong |
|Sender	|2015-05-25 14:58:35	|5/25/2015 8:58:35 PM	|Got Pong from Receiver |

 
## Code walk-through

This sample contains three projects: 

 * Shared - A class library containing shared code including the message definition.
 * Sender - A console application responsible for sending the large message.
 * Receiver - A console application responsible for receiving the large message from Sender.

### Sender project

### Receiver project

### Shared project

