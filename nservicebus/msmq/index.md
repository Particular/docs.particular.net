---
title: MSMQ Information
summary: MSMQ is the primary durable communications technology for Microsoft but does not dynamically detect network interfaces.
tags: []
redirects:
 - nservicebus/msmq-information
---

MSMQ is the default transport used by NServiceBus.

## History

Microsoft Message Queuing (MSMQ) is available on almost all Windows machines from Windows 2000 onwards (though not the Win2K home edition). With Windows XP and Server 2003, Microsoft introduced MSMQ 3.0, and Service Pack 2 brought MSMQ to version 3.1, with an enterprise-ready feature set.

During the time that the Connected Systems Division at Microsoft was developing Indigo (now known as WCF), the intention was to replace MSMQ with something new. Two years later, the effort to replace MSMQ was abandoned and MSMQ was integrated into the standard WCF model and configuration.

Technologies such as BizTalk and SQL Server Service Broker still do not have a .NET API in the Base Class Library (BCL). MSMQ 4.0 was included in the Vista release, showing Microsoft's continued commitment. To this day, MSMQ remains the primary durable communications technology on the Microsoft platform.

## Virtual Private Networks (VPN)

MSMQ isn't smart enough to dynamically detect network interfaces. If you connect a VPN after the MSMQ service starts, you have to restart the MSMQ service for it to detect the VPN. Once it starts with the interface, the VPN is free to disconnect/reconnect whenever it wants.

It is recommended to have batch setup scripts that run on server startups to connect the VPN, which then restarts the MSMQ service automatically.

## MSMQ clustering

MSMQ clustering works by having the active node running the instance of the MSMQ service and the other nodes being cold standbys. On failover, a new instance of the MSMQ service has to be loaded from scratch. All active network connections and associated queue handles break and have to be reconnected. Any transactional processing of messages aborts, returning the message to the queue after startup.

So downtime is proportional to the time taken for the MSMQ service to restart on another node. This is affected by how many messages are in currently storage, awaiting processing.

## Remote Queues
Remote queues are not supported for MSMQ as this conflicts with the Distributed Bus architectural style that is predicated on consents of durability, autonomy and avoiding a single point of failure.
For scenarios where you want to use a Broker Bus style architecture you can use transports like Sql Server and RabbitMQ.

## Useful links

-   [MSMQ Forums](https://social.msdn.microsoft.com/Forums/windowsdesktop/en-US/home?forum=msmq)
-   [MSMQ on Windows Server 2003](https://technet.microsoft.com/en-gb/library/cc757108%28WS.10%29.aspx)
-   [MSMQ on Windows Server 2008](https://technet.microsoft.com/en-gb/library/cc753070%28WS.10%29.aspx)
-   [List of MSMQ articles](http://blogs.msdn.com/b/johnbreakwell/)(very helpful)
-   [Changing the MSMQ Storage location](http://blogs.msdn.com/b/johnbreakwell/archive/2009/02/09/changing-the-msmq-storage-location.aspx) (A better option is virtualization, which ensures that messages are safe.)
-   [Technet content for troubleshooting MSMQ on Windows 2008 and Vista](http://blogs.msdn.com/b/johnbreakwell/archive/2008/05/07/technet-content-for-troubleshooting-msmq-on-windows-2008-and-vista.aspx)
-   [Publicly available tools for troubleshooting MSMQ problems](http://blogs.msdn.com/b/johnbreakwell/archive/2007/12/13/what-publically-available-tools-are-available-for-troubleshooting-msmq-problems.aspx)
-   [MSMQ service might not send or receive messages after you restart a computer that is running Windows 7, Windows Server 2008 R2, Windows Vista, or Windows Server 2008](https://support.microsoft.com/en-us/kb/2554746)
-   [Troubleshooting MSDTC issues with the DTCPing tool](http://blogs.msdn.com/b/distributedservices/archive/2008/11/12/troubleshooting-msdtc-issues-with-the-dtcping-tool.aspx)
