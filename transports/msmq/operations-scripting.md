---
title: MSMQ Transport Scripting
summary: Sample code and scripts to facilitate deployment and operational actions against MSMQ.
reviewed: 2025-01-30
component: MsmqTransport
redirects:
 - nservicebus/msmq/operations-scripting
related:
 - nservicebus/operations
---

This article contains code and scripts to facilitate deployment and operational actions with MSMQ.

These examples use the [System.Messaging](https://docs.microsoft.com/en-us/dotnet/api/system.messaging?view=netframework-4.8) and [System.Transactions](https://docs.microsoft.com/en-us/dotnet/api/system.transactions?view=netframework-4.8) assemblies.

> [!WARNING]
> The `Systems.Messaging` namespace is not available in .NET Core.

> [!NOTE]
> When using the C# code samples, be sure to add the proper includes for both the `System.Messaging` and `System.Transactions` assemblies in the program that's using these functions. When using the PowerShell scripts, include these assemblies by calling `Add-Type` in the script.


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.

> [!NOTE]
> It may be necessary to script the creation of extra instance-specific queues. For example when using [callbacks](/nservicebus/messaging/callbacks.md#message-routing) or scale-out based on [sender-side distribution](/samples/scaleout/senderside/).


See also: [Queue Permissions](/transports/msmq/#permissions)


### The create queue helper methods


#### In C&#35;

snippet: msmq-create-queues


#### In PowerShell

snippet: msmq-create-queues-powershell


### Creating queues for an endpoint

To create all queues for a given endpoint name.


#### In C&#35;

snippet: msmq-create-queues-for-endpoint


#### In PowerShell

snippet: msmq-create-queues-for-endpoint-powershell


### Using the create endpoint queues


#### In C&#35;

snippet: msmq-create-queues-endpoint-usage


#### In PowerShell

snippet: msmq-create-queues-endpoint-usage-powershell


### To create shared queues


#### In C&#35;

snippet: msmq-create-queues-shared-usage


#### In PowerShell

snippet: msmq-create-queues-shared-usage-powershell


## Delete queues


### The delete helper queue methods


#### In C&#35;

snippet: msmq-delete-queues


#### In PowerShell

snippet: msmq-delete-queues-powershell


### To delete all queues for a given endpoint

snippet: msmq-delete-queues-for-endpoint

snippet: msmq-delete-queues-endpoint-usage


### To delete shared queues

snippet: msmq-delete-queues-shared-usage
