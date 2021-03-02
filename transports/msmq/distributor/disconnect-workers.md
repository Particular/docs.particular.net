---
title: Disconnect Workers
summary: How a worker can be disconnected from its distributor using PowerShell cmdlets
reviewed: 2021-03-02
redirects:
 - nservicebus/disconnect-workers-from-running-distributor
 - nservicebus/scalability-and-ha/disconnect-workers
 - nservicebus/msmq/distributor/disconnect-workers
---

The Distributor starts sending messages to a Worker once it is aware of it. A Worker registers itself with a Distributor by sending a message containing a `SessionID` that identifies the current running Worker and the number of messages it can handle concurrently.

## Prerequisites

Prior to installation ensure that PowerShell 2 or higher is installed. NServiceBus PowerShell modules are compatible with PowerShell 5. Versions of PowerShell later than 5 (including PowerShell Core) are not supported and might not work as expected.

## Disconnecting a Worker

If the Worker is configured using the [NServiceBus.Distributor.MSMQ NuGet](https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ), there is a PowerShell cmdlet that can be used to disconnect a Worker from a Distributor. The steps are the following:

 1. Load the [NServiceBus PowerShell CmdLet](/nservicebus/operations/management-using-powershell.md) and execute
 ```ps
 Remove-NServiceBusMSMQWorker WorkerAddress DistributorAddress TransactionalDistributorQueue
 ```
 Where:
   * `WorkerAddress` is the Worker queue name, eg `Worker@localhost`
   * `DistributorAddress` is the Distributor queue name eg `MyDistributor@localhost`, **Note:** Pass the Distributor queue name, the PowerShell cmdlet will automatically appends ".distributor.control" to the end of the Distributor queue.
   * `TransactionalDistributorQueue` is the Distributor queue transactional or not ?
 1. Wait for Worker to drain all queued messages from its input queue.
 1. Shutdown the endpoint.


## Distributor behavior after the PowerShell cmdlet is executed

 1. A disconnect message is sent by the PowerShell cmdlet to the Distributor control queue.
 1. When the Distributor processes it, the Worker with the address specified in the message is set with SessionID `disconnected`.
 1. Ready messages sent back by the Worker to the Distributor never match the session, so they are skipped and that way the Worker won't receive any more messages from the Distributor.
