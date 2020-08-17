---
title: MSMQ Transport
summary: MSMQ is a solid durable communications technology but does not dynamically detect network interfaces.
component: MsmqTransport
related:
 - samples/msmq/simple
 - samples/msmq/persistence
 - samples/msmq/dlq-customcheck
 - samples/msmq/msmqtosqlrelay
reviewed: 2020-01-30
redirects:
 - nservicebus/msmq-information
 - nservicebus/msmq
---

WARNING: As Microsoft is not making MSMQ available for .NET Core, building new systems using MSMQ is not recommended. 

## Transport at a glance

|Feature                    |   |  
|:---                       |---
|Transactions |None, ReceiveOnly, SendsWithAtomicReceive, TransactionScope
|Pub/Sub                    |message driven
|Timeouts                   |via timeouts storage
|Large message bodies       |via data bus
|Scale-out             |Distributor or Windows Network Load Balancing
|Scripted Deployment        |C#, PowerShell
|Installers                 |Optional

## Configuring the endpoint

partial: default


## Advantages

 * MSMQ is natively available as part of the Windows operating system. In Windows servers, the MSMQ role might need to be turned on.  
 * MSMQ offers transactional queues which also support distributed transactions. With the transactional behavior, it is possible to get exactly-once delivery.
 * MSMQ provides a store and forward mechanism. Therefore it promotes a more natural [bus-style architecture](/transports/types.md#federated-transports). When sending messages to unavailable servers, the messages are stored locally in the outgoing queues and will be delivered automatically once the machine comes back online.
 

## Disadvantages

 * MSMQ does not offer a native publish-subscribe mechanism, therefore it requires NServiceBus persistence to be configured for storing event subscriptions. [Explicit routing for publish/subscribe](/nservicebus/messaging/routing.md#event-routing-message-driven) must also be specified.
 * Many organizations don't have the same level of operational expertise with MSMQ that they do with other technologies (e.g. SQL Server), so it may require additional training. For example, as MSMQ is not a broker transport, the messages could be on different servers, and managing the storage space on each machine is important.
 * As MSMQ is a store and forward transport, it requires more setup for load balancing. I.e. it requires either a [distributor or sender side distribution](/transports/msmq/scaling-out.md) to be configured.
 * MSMQ (i.e. the `System.Messaging` namespace) is not available on .NET Core


## MSMQ configuration

NServiceBus requires a specific MSMQ configuration to operate.

The supported configuration is to have only the base MSMQ service installed with no optional features. To enable the supported configuration, use the `Install-NServiceBusMSMQ` cmdlet from the [NServiceBus PowerShell Module](/nservicebus/operations/management-using-powershell.md).

Alternatively, the MSMQ service can be installed manually. When installing manually **do not** enable the following components:

 * MSMQ Active Directory Domain Services Integration
 * MSMQ HTTP Support
 * MSMQ Triggers
 * Multicasting Support
 * MSMQ DCOM Proxy

These components can cause issues with the addressing used in NServiceBus.

### Installation on Windows Server 2012 and higher

From Server Manager's Add Roles and Features Wizard, enable `Message Queue Server`. All other MSMQ options should be disabled.

The DISM command line equivalent is:

```dos
DISM.exe /Online /NoRestart /English /Enable-Feature /all /FeatureName:MSMQ-Server
```

### Installation on Windows 10

From the Control Panel, choose Programs. Then run the Windows Features Wizard by clicking `Turn Windows Features On or Off`. Enable `Microsoft Message Queue (MSMQ) Server Core`. All other MSMQ options should be disabled.

The DISM command line equivalent is:

```dos
DISM.exe /Online /NoRestart /English /Enable-Feature /all /FeatureName:MSMQ-Server
```


## MSMQ machine name limitation

For MSMQ to function properly, [the server name must be 15 characters or less](http://geekswithblogs.net/Plumbersmate/archive/2012/02/03/make-sure-computer-names-are-15-characters-or-less-fro.aspx). This is because of a NETBIOS limitation.


## MSMQ clustering

MSMQ clustering works by having the active node running an instance of the MSMQ service and the other nodes being cold standbys. On failover, a new instance of the MSMQ service must be loaded from scratch. Otherwise, all active network connections and associated queue handles will break and need to be reconnected. Any transactional processing of messages aborts, returning the message to the queue after startup.

Downtime is proportional to the time taken for the MSMQ service to restart on another node. This is affected by how many messages are awaiting processing in current storage.


## Remote queues

Remote queues are not supported for MSMQ as this conflicts with the [distributed bus architectural style](/nservicebus/architecture/) that is predicated on concepts of durability, autonomy and avoiding a single point of failure. For scenarios where a [broker style architecture](/nservicebus/architecture/) is required, use transports like [SQL Server](/transports/sql/) and [RabbitMQ](/transports/rabbitmq/).


## Error queue configuration

The transport requires all endpoints to configure the error queue address. The centralized error queue is a recommended setup for production scenarios.

See the [recoverability documentation](/nservicebus/recoverability/configure-error-handling.md) for details on how to configure the error queue address.


## Public queues

Although MSMQ has the concept of both [public and private queues](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/cc753440(v=ws.10)), public queues require Active Directory as a prerequisite and are not available in a workgroup environment. Therefore, NServiceBus supports only private queues and uses the path name addressing scheme for its routing. Installing MSMQ with Active Directory may interfere with the addressing scheme when sending messages and for this reason, it is recommended not to include Active Directory when installing MSMQ.


## Permissions

| Group          | Permissions         | Granted by NServiceBus   | Granted by Windows 2012+ |
|----------------|---------------------|--------------------------|--------------------------|
| Owning account | Send, Receive, Peek | All versions             | Domain & Workgroup mode  |
| Administrators | Full                | All versions             | None                     |
| Anonymous      | Send                | Versions 6.0.x and below | Workgroup mode           |
| Everyone       | Send                | Versions 6.0.x and below | Workgroup mode           |

NOTE: In NServiceBus version 6.1.0 and above, the NServiceBus installers will not automatically grant permissions to the `Anonymous` and `Everyone` group. The installer will respect the existing queue permissions that have been set up for the endpoint queue. The permissions granted to `Anonymous` and `Everyone` groups are based on standard Windows behavior.

Any endpoint that sends a message to a target endpoint requires the `Send` permission to be granted for the sending user account on the target queue. For example, if an `endpoint A` is running as `userA` and is sending a message to `endpoint B`, then `userA` requires the `Send` permission to be granted on `endpoint B`'s queue. When using messaging patterns like request-response or publish-subscribe, the queues for both endpoints will require `Send` permissions to be granted to each user account. 

When an endpoint creates a queue on a machine, permissions depend on whether the server is joined to a [domain or a workgroup](https://support.microsoft.com/en-us/help/884974/information-about-workgroup-mode-and-about-domain-mode-in-microsoft-me) due to Windows behavior. 


### Domain mode

If the machine is part of a domain, then at the time of queue creation, only the domain user that created the queue will have `Send` permissions granted. The `Everyone` user group and `Anonymous` user group will NOT have `Send` permissions. If all the endpoints which need to communicate are running under the same domain account, no further configuration is required. However, if the endpoints are run using different domain accounts, the `Send` permission on the receiving endpoint's input queue must be explicitly granted to the domain user account of the sending endpoint.


### Workgroup mode

If the machine is connected to a workgroup, the `Send` permission is granted to the `Everyone` and `Anonymous` user groups by Windows. Any endpoint will be able to send messages to any other endpoint without further configuration.


### Well-known group names and queue access rights
The [WellKnownSidType](https://docs.microsoft.com/en-us/dotnet/api/system.security.principal.wellknownsidtype?view=netframework-4.8) enumeration is used to retrieve the group names.

MSMQ permissions are defined in the [MessageQueueAccessRights](https://docs.microsoft.com/en-us/dotnet/api/system.messaging.messagequeueaccessrights?view=netframework-4.8) enumeration.

NOTE: To increase security and further lock down MSMQ send/receive permissions, remove `Everyone` and `Anonymous` and grant specific permissions to the subset of accounts that need them.

NOTE: In NServiceBus version 6 and above, if the default queue permissions are set, a log message will be written during the endpoint startup indicating that the queue has default permissions and might require stricter permissions for production. During development, if running with an attached debugger, this message will be logged at an `INFO` level, otherwise `WARN`.

An example of the warning that is logged:

> WARN NServiceBus.QueuePermissions - Queue [private$\xxxx] is running with [Everyone] with AccessRights set to [GenericWrite]. Consider setting appropriate permissions, if required by the organization. For more information, consult the documentation.

See also [Message Queuing Security Overview](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/cc771268(v=ws.10)).

## Distributed Transaction Coordinator

NServiceBus makes use of the Microsoft Distributed Transaction Coordinator (MSDTC) to synchronize transactions between MSMQ and the database in order to support [guaranteed once delivery of messages](/nservicebus/operations/transactions-message-processing.md). For this to work, MSDTC must be started and configured correctly. This can be done manually or using the [NServiceBus PowerShell module](/nservicebus/operations/management-using-powershell.md).

Alternatively, there is a _non-MSDTC mode of operation available. In this mode NServiceBus uses the _outbox_, a message store backed by the same database as the user code, to temporarily store messages that must be sent as a result of processing an incoming message. To read more about this subject see [Outbox](/nservicebus/outbox/).

If neither the MSDTC nor the outbox is configured, an exception message will appear when an MSMQ-enabled endpoint is started:

```
Transaction mode is set to `TransactionScope`. This depends on Microsoft Distributed Transaction Coordinator (MSDTC) which is not available. Either enable MSDTC, enable Outbox, or lower the transaction mode to `SendsAtomicWithReceive`.
```
