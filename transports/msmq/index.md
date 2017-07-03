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

| Group          | Permissions         | Description                       |
|----------------|---------------------|-----------------------------------|
| Owning account | Send, Receive, Peek | Set by NServiceBus                |
| Administrators | Full                | Set by NServiceBus                |
| Anonymous      | Send                | Set by NServiceBus prior to 6.1.0 |
| Everyone       | Send                | Set by NServiceBus prior to 6.1.0 |


When an endpoint sends a command or event message that source requires permission at the target queue. For endpoints using the either the publish-subscribe or request-response patterns, both parties actually send messages and must have permission to send messages to each other. This is to allow the Subscribe message to be sent from the subscribing endpoint to the publishing endpoint in case of publish-subscribe or the response to be send back to the client in case of request-response.

NOTE: In versions 5.2.20 and 6.0.3 a bug was fixed where installers always changed permissions on a queue, also if it already existed.

When an NServiceBus endpoint creates a queue on a machine, the default permissions depend on whether the machine is connected to a domain or a workgroup.

### Domain mode

If the machine is connected to a domain, then only the domain user running the endpoint when the queue is created will have Send permissions granted. If multiple endpoints need to communicate and are running under the same domain account, no further configuration is required. If multiple endpoints need to communicate using different domain accounts then the Send permission on the receiving endpoint input queue needs to be granted to the domain account of the sending endpoint.

### Workgroup mode

If the machine is connected to a workgroup then the Send permission is granted to the Everyone and Anonymous user groups by Windows. Any endpoint will be able to send messages to any other endpoint without further configuration.


To retrieve the group names the [WellKnownSidType](https://msdn.microsoft.com/en-us/library/system.security.principal.wellknownsidtype.aspx) enumeration is used.

MSMQ permissions are defined in the [MessageQueueAccessRights](https://msdn.microsoft.com/en-us/library/system.messaging.messagequeueaccessrights.aspx) enumeration.

NOTE: To increase security and further lock down MSMQ send/receive permissions remove `Everyone` and `Anonymous` and grant specific permissions to the subset of accounts that need them.

NOTE: From Version 6, if the default queue permissions are set, a log message will be written during the transport startup, reminding that the queue has default permissions. During development, if running with an attached debugger, this message will be logged as `INFO` level, otherwise `WARN`.

Example of the warning that is logged:

> WARN NServiceBus.QueuePermissions - Queue [private$\xxxx] is running with [Everyone] with AccessRights set to [GenericWrite]. Consider setting appropriate permissions, if required by the organization. For more information, consult the documentation.

See also [Message Queuing Security Overview in Windows Server 2008](https://technet.microsoft.com/en-us/library/cc771268.aspx).
