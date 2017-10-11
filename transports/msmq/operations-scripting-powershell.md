---
title: Scripting using PowerShell
summary: Example code and scripts to facilitate deployment and operational actions against MSMQ.
reviewed: 2016-10-11
component: MsmqTransport
redirects:
 - nservicebus/msmq/operations-scripting
related:
 - nservicebus/operations
---

INFO: These PowerShell scripts use [System.Messaging.dll](https://msdn.microsoft.com/en-us/library/System.Messaging.aspx) and [System.Transactions.dll](https://msdn.microsoft.com/en-us/library/system.transactions.aspx) assemblies. Therefore it is important to add the type includes for these assemblies in the beginning of the scripts.

```
Add-Type -AssemblyName System.Messaging
Add-Type -AssemblyName System.Transactions
```

## Native Send

include: operations-scripting-send


### The native send helper methods

snippet: msmq-nativesend-powershell


### Usage

snippet: msmq-nativesend-powershell-usage


## Return message to source queue


### The retry helper methods

include: operations-scripting-retry

snippet: msmq-return-to-source-queue-powershell


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.

partial: extra-queues


See also: [Queue Permissions](/transports/msmq/#permissions)


### The create queue helper methods

snippet: msmq-create-queues-powershell


### Creating queues for an endpoint

To create all queues for a given endpoint name.

snippet: msmq-create-queues-for-endpoint-powershell


### Usage

snippet: msmq-create-queues-endpoint-usage-powershell


### To create shared queues

snippet: msmq-create-queues-shared-usage-powershell


## Delete queues


### The delete helper queue methods

snippet: msmq-delete-queues-powershell