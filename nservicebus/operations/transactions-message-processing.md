---
title: Transactions and Message Processing
summary: Overview of message processing, transactions and consistency guarantees in NServiceBus.
reviewed: 2016-08-05
redirects:
- nservicebus/transactions-message-processing
- nservicebus/operations/warning-for-cid-values
related:
- nservicebus/outbox
- nservicebus/recoverability
---

NServiceBus offers four levels of consistency guarantees with regards to message processing. Levels availability depends on the selected transport. The default consistency level is TransactionScope (Distributed Transaction), but a different level can be specified using code configuration API. 

See the [Transports Transactions](/transports/transactions.md) article to learn more about NServiceBus consistency guarantees.

By default, the transaction timeout limit is set to 10 minutes. See the [Override the System.Transactions default timeout of 10 minutes](https://blogs.msdn.microsoft.com/ajit/2008/06/18/override-the-system-transactions-default-timeout-of-10-minutes-in-the-code/) article to learn how to adjust that value.


## Distributed Transaction Coordinator

In Windows, the Distributed Transaction Coordinator (DTC) is an OS-level service which manages transactions that span across multiple resources, e.g. queues and databases.

The easiest way to configure DTC for NServiceBus is to run the [PlatformInstaller](/platform/installer/) for NServiceBus, or to use the dedicated [PowerShell commandlets](/nservicebus/operations/management-using-powershell.md).


### Troubleshooting Distributed Transaction Coordinator

The [DTCPing](https://www.microsoft.com/en-us/download/details.aspx?id=2868) tool is very useful for verifying that DTC service is configured correctly, as well as for troubleshooting:

![DTCPing tool](dtcping.png "DTCPing tool")

For more information refer to the [Troubleshooting MSDTC issues with the DTCPing tool](https://blogs.msdn.microsoft.com/puneetgupta/2008/11/12/troubleshooting-msdtc-issues-with-the-dtcping-tool/) article on MSDN.

NOTE: If the `DTCPing WARNING: The CID Values for Both Test Machines Are the Same` message appears when running the DTCPing tool, the reason might be that the machine name is longer than 14 characters. For DTCPing and MSDTC to work, the machine name should be shorter than 15 characters.


## Message processing loop

Messages are processed in NServiceBus in the following steps:

 1. The queue is peeked to see if there's a message.
 1. If there's a message then transaction is started.
 1. The queue is contacted again to receive a message. This is because multiple threads may have peeked the same message. The queue makes sure only one thread actually gets a given message.
 1. If the thread is able to get it, NServiceBus tries to deserialize the message. If it fails, the message moves to the configured error queue and the transaction commits.
 1. After a successful deserialization, NServiceBus invokes all infrastructure, message mutators and handlers. An exception in this step causes the transaction to roll back and the message to return to the input queue. The message will be re-sent for a configured number of times, if all attempts fail then it'll be moved to the error queue.

Refer to the [Message Handling Pipeline](/nservicebus/pipeline/) article to learn more about message processing.

Refer to the [Recoverability](/nservicebus/recoverability/) and the [ServicePulse: Failed Message Monitoring](/servicepulse/intro-failed-messages.md) articles to learn more about error handling, automatic and manual retries, as well as processing failures monitoring.