---
title: Transactions and Message Processing
summary: Fault-Tolerant by Default infrastructure saves remembering the configuration of threading and state management elements.
reviewed: 2016-08-05
redirects:
- nservicebus/transactions-message-processing
related:
- nservicebus/outbox
- nservicebus/recoverability
---

NServiceBus offers four levels of consistency guarantees with regards to message processing. Levels availability depends on the selected transport. The default consistency level is TransactionScope (Distributed Transaction), but a different level can be specified using code configuration API. 

See the [Transports Transactions](/nservicebus/transports/transactions.md) article to learn more about NServiceBus consistency guarantees.

By default, the transaction timeout limit is set to 10 minutes. See the [Override the System.Transactions default timeout of 10 minutes](https://blogs.msdn.microsoft.com/ajit/2008/06/18/override-the-system-transactions-default-timeout-of-10-minutes-in-the-code/) article to learn how to adjust that value.


## Distributed Transaction Coordinator

In Windows, the Distributed Transaction Coordinator (DTC) is an OS-level service which manages transactions that span across multiple resources, e.g. queues and databases.

The easiest way to configure DTC for NServiceBus is to run the [PlatformInstaller](http://docs.particular.net/platform/installer/) for NServiceBus, or to use the dedicated [Powershell commandlets](/nservicebus/operations/management-using-powershell).


## Message processing loop

Messages are processed in NServiceBus in the following steps:

 1. The queue is peeked to see if there's a message.
 1. If there's a message then transaction is started.
 1. The queue is contacted again to receive a message. This is because multiple threads may have peeked the same message. The queue makes sure only one thread actually gets a given message.
 1. If the thread is able to get it, NServiceBus tries to deserialize the message. If it fails, the message moves to the configured error queue and the transaction commits.
 1. After a successful deserialization, NServiceBus invokes all infrastructure, message mutators and handlers. An exception in this step causes the transaction to roll back and the message to return to the input queue. The message will be re-sent for a configured number of times, if all attempts fail then it'll be moved to the error queue.

Refer to the [Message Handling Pipeline](/nservicebus/pipeline/) article to learn more about message processing.

Refer to the [Recoverability](/nservicebus/recoverability/) and the [ServicePulse: Failed Message Monitoring](/servicepulse/intro-failed-messages.md) articles to learn more about error handling, automatic and manual retries, as well as processing failures monitoring.


## Troubleshooting Distributed Transaction Coordinator

The [DTCPing](https://www.microsoft.com/en-us/download/details.aspx?id=2868) tool is very useful for verifying that DTC service is configured correctly, as well as for troubleshooting:

![this is what the initial DTCPing window to look like.](dtcping.png "this is what the initial DTCPing window to look like.")

It can be used to verify if one machine can access a remote machine over the DTC, simply enter the name of the remote server in the "Remote Server Name" and click the "Ping" button. 

If an error referring to the RPC Endpoint Mapper occurs, run `dcomcnfg` command in the command prompt. That will open the Component Services screen:

![this is the component services and dtc configuration](dtc-dcomcnfg-1.png "this is the component services and dtc configuration")

Open some ports by right clicking "My Computer" and going to the "Default Protocols" tab. From there, select "Connection-oriented TCP/IP" and click the "Properties" button. In the "Properties for COM Internet Services" dialog, check that the Port Ranges includes "5000-6000", as shown:

![](dtc-dcomcnfg-2.png)

If the list of Port Ranges is empty, click the "Add..." button and enter "5000-6000" in the dialog box, as shown above. It is also possible have less than 1000 open ports, the optimal number depends on the number of machines connecting to each other over the DTC.

After clicking OK and returning to the Component Services screen, navigate to the "Local DTC" node under the Distributed Transaction Coordinator folder, right click, and select "Properties". In the dialog that opens, select the Security tab:

![dtc security settings](dtc-dcomcnfg-3.png "dtc security settings")

Ensure that the properties are the same as shown in the screenshot above and restart the computer.

If DTCPing isn't working, check that the needed ports are open in the firewall.

If DTCPing shows a message about finding the name but not being able to reach it, perform a simple ping by running `ping computername` in the command prompt. If the machine cannot be reached by ping, it could be a DNS problem that may require a network administrator's help.

Make sure to perform all the steps not just on the servers that connect to the database, but also on the database servers as well.

Finally, make sure that each server uses a different TCP port, because communication is bi-directional.