---
title: Multi-Instance Mode
summary: SQL Server transport running in multi-instance mode
reviewed: 2018-05-18
component: SqlTransport
related:
 - transports/sql/deployment-options
---

NOTE: In SQL Server transport version 4, multi-instance mode has been deprecated. The [migration sample](/samples/sqltransport/multi-instance-migration) explains how to use the [transport bridge](/nservicebus/bridge/) instead.

## Prerequisites

include: sql-prereq

The databases created by this sample are `NsbSamplesSqlMultiInstanceReceiver` and `NsbSamplesSqlMultiInstanceSender`.

Ensure [Distributed Transaction Coordinator (DTC)](https://msdn.microsoft.com/en-us/library/ms684146.aspx) is running. It can be started from the command line by running `net start msdtc`.


## Running the project

 1. Start both projects.
 1. Press <kbd>enter</kbd> in the Sender's console window to send a new message.


## Verifying that the sample works correctly

 1. The Receiver displays information that an order was submitted.
 1. The Sender displays information that the order was accepted.


## Code walk-through

The sample contains the following projects:

 * Sender: A console application responsible for sending the initial `ClientOrder` message and processing the follow-up `ClientOrderAccepted` message.
 * Receiver: A console application responsible for processing the order message.
 * Messages: A class library containing message definitions.

partial: passconnection


### Sender project

The Sender does not store any data. It mimics a front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use the SQL Server transport and run in [*multi-instance*](/transports/sql/deployment-options.md?version=SqlTransport_3#multi-instance.md) mode. `ConnectionProvider.GetConnection` method is used for providing connections.

snippet: SenderConfiguration

The Sender sends a message to the Receiver:

snippet: SendMessage


### Receiver project

The Receiver mimics a back-end system. It is configured to use the SQL Server transport in [*multi-instance*](/transports/sql/deployment-options.md?version=SqlTransport_3#multi-instance.md) mode.

snippet: ReceiverConfiguration

It receives `ClientOrder` messages sent by Sender and replies to them with `ClientOrderAccepted`.

snippet: Reply


### Multi-instance connection lookup

Both the sender and receiver provide a custom lookup mechanism for providing connection information for a given destination. The following snippet shows lookup logic used by the sender.

snippet: SenderConnectionProvider


## How it works

The sender and receiver use [different catalogs](/transports/sql/deployment-options.md) on the same SQL Server instance. The tables representing queues for a particular endpoint are created in the appropriate catalog, i.e. in `NsbSamplesSqlMultiInstanceReceiver` for the receiver endpoint and in `NsbSamplesSqlMultiInstanceSender` for the sender endpoint. It is possible to register a custom `SqlConnection` factory that provides connection instance per given transport address. The operations performed on queues stored in different catalogs are atomic because SQL Server allows multiple `SqlConnection` enlisting in a single distributed transaction.

NOTE: In this sample DTC is required by the receiver because it operates on two different catalogs when receiving a request from the sender. It picks a message from the input queue stored in `NsbSamplesSqlMultiInstanceReceiver` and sends a reply to the sender's input queue stored in `NsbSamplesSqlMultiInstanceSender`. In addition the `error` queue is also stored in `NsbSamplesSqlMultiInstanceSender` so without DTC, the receiver will not be able to handle failed messages properly.
