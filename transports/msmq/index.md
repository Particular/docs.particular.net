---
title: MSMQ Transport
summary: MSMQ is the primary durable communications technology for Microsoft but does not dynamically detect network interfaces.
component: MsmqTransport
reviewed: 2016-08-31
redirects:
 - nservicebus/msmq-information
 - nservicebus/msmq
---

partial: default


## NServiceBus Configuration

NServiceBus requires a specific MSMQ configuration to operate.

The supported configuration is to only have the base MSMQ service installed with no optional features. To enable the supported configuration either use `NServiceBus Prerequisites` in the [Platform Installer](/platform/installer/) or use the `Install-NServiceBusMSMQ` cmdlet from the [NServiceBus PowerShell Module](/nservicebus/operations/management-using-powershell.md).

Alternatively the MSMQ service can be manually installed:


### Windows 2012

From Server Manager's Add Roles and Features Wizard enable `Message Queue Server`. All other MSMQ options should be disabled.

The DISM command line equivalent is:

```dos
DISM.exe /Online /NoRestart /English /Enable-Feature /all /FeatureName:MSMQ-Server
```


### Windows 8.x and 10

From the Control Panel, choose Programs. Then run the Windows Features Wizard by click on `Turn Windows Features On or Off`. Enable `Microsoft Message Queue (MSMQ) Server Core`. All other MSMQ sub-options should be disabled.

The DISM command line equivalent is:

```dos
DISM.exe /Online /NoRestart /English /Enable-Feature /all /FeatureName:MSMQ-Server
```


## MSMQ Machine name limitation

For MSMQ to function properly, [the server name should be 15 characters or less](http://geekswithblogs.net/Plumbersmate/archive/2012/02/03/make-sure-computer-names-are-15-characters-or-less-fro.aspx). This is because of [NETBIOS limitation](https://support.microsoft.com/en-us/help/163409/netbios-suffixes-16th-character-of-the-netbios-name). Having a longer machine name may result in MSMQ not functioning properly.


## MSMQ clustering

MSMQ clustering works by having the active node running the instance of the MSMQ service and the other nodes being cold standbys. On failover, a new instance of the MSMQ service has to be loaded from scratch. All active network connections and associated queue handles break and have to be reconnected. Any transactional processing of messages aborts, returning the message to the queue after startup.

So downtime is proportional to the time taken for the MSMQ service to restart on another node. This is affected by how many messages are in currently storage, awaiting processing.


## Remote Queues

Remote queues are not supported for MSMQ as this conflicts with the [Distributed Bus architectural style](/nservicebus/architecture/) that is predicated on concepts of durability, autonomy and avoiding a single point of failure. For scenarios where a [Broker style architecture](/nservicebus/architecture/) is required use transports like [Sql Server](/transports/sqlserver/) and [RabbitMQ](/transports/rabbitmq/).


## Error queue configuration

The transport requires all endpoints to configure the error queue address. The centralized error queue is a recommended setup for production scenarios.

See the [recoverability documentation](/nservicebus/recoverability/configure-error-handling.md) for details on how to configure the error queue address.


## Public Queues

Although MSMQ has the concept of both [Public and Private queues](https://technet.microsoft.com/en-us/library/cc753440.aspx). Public queues require Active Directory as a pre-requisite and are also not available in a workgroup environment. Therefore, NServiceBus only supports private queues and uses the path name addressing scheme for all its routing.  Installing MSMQ with Active Directory may in some cases interfere with the addressing scheme when sending messages and for this reason, it is recommended not to include Active Directory when installing MSMQ.


## Permissions

| Group          | Permissions         | Description                                    |
|----------------|---------------------|------------------------------------------------|
| Owning account | Send, Receive, Peek | Set by NServiceBus                             |
| Administrators | Full                | Set by NServiceBus                             |
| Anonymous      | Send                | Set by NServiceBus in Versions 6.0.x and below |
| Everyone       | Send                | Set by NServiceBus in Versions 6.0.x and below |

NOTE: In Versions 6.1.0 and above, the installers will not automatically grant permissions to the `Anonymous` and `Everyone` group. It will respect the existing queue permissions that have been set up for the endpoint. The permissions granted to these user accounts is based on standard windows behavior and not via NServiceBus.

Any endpoint that sends a message to a target endpoint requires the `Send` permission to be granted for the sending user account on the target queue. For example, if an `endpoint A` is running as `userA` and is sending a message to `endpoint B`, then `userA` requires the `Send` permission to be granted on `endpoint B`'s queue. When using messaging patterns like request-response or publish-subscribe, the queues for both the endpoints will require `Send` permissions to be granted to each others user accounts. 

When an endpoint creates a queue on a machine, the default permissions depend on whether the server is joined to a [domain or a workgroup](https://support.microsoft.com/en-us/help/884974/information-about-workgroup-mode-and-about-domain-mode-in-microsoft-me).


### Domain mode

If the machine is joined to a domain, then at the time of queue creation, only the domain user that created the queue, domain administrators and local administrators will have `Send` permissions granted. The `Everyone` user group and `Anonymous` user group will not have `Send` permissions. If all the endpoints which need to communicate are running under the same domain account, no further configuration is required. However, if the endpoints are run using different domain accounts, then the `Send` permission on the receiving endpoint's input queue needs to be explicitly granted to the domain user account of the sending endpoint.


### Workgroup mode

If the machine is connected to a workgroup, then the `Send` permission is granted to the `Everyone` and `Anonymous` user groups by Windows. Any endpoint will be able to send messages to any other endpoint without further configuration.


### Well-Known group names and Queue access rights
To retrieve the group names the [WellKnownSidType](https://msdn.microsoft.com/en-us/library/system.security.principal.wellknownsidtype.aspx) enumeration is used.

MSMQ permissions are defined in the [MessageQueueAccessRights](https://msdn.microsoft.com/en-us/library/system.messaging.messagequeueaccessrights.aspx) enumeration.

NOTE: To increase security and further lock down MSMQ send/receive permissions remove `Everyone` and `Anonymous` and grant specific permissions to the subset of accounts that need them.

NOTE: In Versions 6 and above, if the default queue permissions are set, a log message will be written during the endpoint startup, reminding that the queue has default permissions and might require stricter permissions for production. During development, if running with an attached debugger, this message will be logged as `INFO` level, otherwise `WARN`.

An example of the warning that is logged:

> WARN NServiceBus.QueuePermissions - Queue [private$\xxxx] is running with [Everyone] with AccessRights set to [GenericWrite]. Consider setting appropriate permissions, if required by the organization. For more information, consult the documentation.

See also [Message Queuing Security Overview in Windows Server 2008](https://technet.microsoft.com/en-us/library/cc771268.aspx).
