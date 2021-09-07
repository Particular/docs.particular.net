---
title: MSMQ Native Integration
summary: Sample code and scripts to facilitate native integration scenario's with MSMQ.
reviewed: 2021-09-07
component: MsmqTransport
---

## Native integration

This document describes how to consume messages from and send messages to non-NServiceBus endpoints via MSMQ in integration scenarios.

These examples use the [System.Messaging](https://docs.microsoft.com/en-us/dotnet/api/system.messaging?view=netframework-4.8) and [System.Transactions](https://docs.microsoft.com/en-us/dotnet/api/system.transactions?view=netframework-4.8) assemblies.

WARNING: The `Systems.Messaging` namespace is not available in .NET Core.

INFO: When using the C# code samples, be sure to add the proper includes for both the `System.Messaging` and `System.Transactions` assemblies in the program that's using these functions. When using the PowerShell scripts, include these assemblies by calling `Add-Type` in the script.



## Native send

### The native send helper methods

Sending involves the following actions:

* Creating and serializing headers.
* Writing a message body directly to MSMQ.


#### In C&#35;

snippet: msmq-nativesend


#### In PowerShell

snippet: msmq-nativesend-powershell


### Using the native send helper methods


#### In C&#35;

snippet: msmq-nativesend-usage


#### In PowerShell

snippet: msmq-nativesend-powershell-usage


## Return message to source queue


### The retry helper methods

Retrying a message involves the following actions:

* Reading a message from the error queue.
* Extracting the failed queue from the headers.
* Forwarding that message to the failed queue name so it can be retried.


#### In C&#35;

snippet: msmq-return-to-source-queue


#### In PowerShell

snippet: msmq-return-to-source-queue-powershell


### Using the retry helper methods

snippet: msmq-return-to-source-queue-usage
