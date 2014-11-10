---
title: Disconnect Workers from a running Distributor
summary: How a worker can be disconnected from its distributor using PowerShell cmdlets
tags: 
- Scalability
- Distributor
---

NServiceBus Distributor starts sending messages to a Worker once it is aware of it. A Worker registers itself with a Distributor by sending a message containing a SessionID that identifies the current running Worker and the number of messages it can handle concurrently.

## How a Worker can be disconnected?

If the Worker is configured using the [NServiceBus.Distributor.MSMQ NuGet](https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ), there is a PowerShell cmdlet that can be used to disconnect a Worker from a Distributor. The steps are the following:

1. Load the [NServiceBus PowerShell CmdLet](managing-nservicebus-using-powershell.md) and execute
```ps
Remove-NServiceBusMSMQWorker WorkerAddress DistributorAddress
```
{{NOTE:
   * `WorkerAddress` is the Worker queue name, eg Worker@localhost
   * `DistributorAddress` is the Distributor queue name eg MyDistributor@localhost, Note: you just pass the Distributor queue name, the PowerShell cmdlet will automatically appends ".distributor.control" to the end of the Distributor queue.
}} 
2. Wait for Worker to drain all queued messages from its input queue.
3. Shutdown the endpoint.


## What is happening inside the Distributor after the PowerShell is executed?

1. A disconnect message is sent by the PowerShell to the Distributor control queue.
2. When the Distributor processes it, the Worker with the address specified in the message is set with SessionID  "disconnected".
3. Ready messages sent back by the Worker to the Distributor never match the session, so they are skipped and that way the Worker won't receive any more messages from the Distributor.



