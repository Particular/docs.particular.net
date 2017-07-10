---
title: Multi-Instance Mode
summary: SqlServer transport running in Multi-Instance Mode
reviewed: 2016-09-13
component: SqlTransport
related:
 - transports/sql/deployment-options
---


## Prerequisites

 1. Make sure SQL Server Express is installed and accessible as `.\SqlExpress`.
 1. Create two databases called `receivercatalog` and `sendercatalog`.
 1. Make sure that [Distributed Transaction Coordinator (DTC)](https://msdn.microsoft.com/en-us/library/ms684146.aspx) is running. It can be started from the command line by running `net start msdtc`.


## Running the project

 1. Start both projects.
 1. Hit enter in Sender's console window to send a new message.


## Verifying that the sample works correctly

 1. The Receiver displays information that an order was submitted.
 1. The Sender displays information that the order was accepted.


## Code walk-through

This sample contains the following projects:

 * Sender - A console application responsible for sending the initial `ClientOrder` message and processing the follow-up `ClientOrderAccepted` message.
 * Receiver - A console application responsible for processing the order message.
 * Messages - A class library containing message definitions.

partial: passconnection


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use SQL Server transport and run in the [*multi-instance*](/transports/sql/deployment-options.md#modes-overview-multi-instance) mode. `ConnectionProvider.GetConnection` method is used for providing connections.

snippet: SenderConfiguration

The Sender sends a message to the Receiver:

snippet: SendMessage


### Receiver project

The Receiver mimics a back-end system. It is configured to use SQLServer transport in the [*multi-instance*](/transports/sql/deployment-options.md#modes-overview-multi-instance) mode.

snippet: ReceiverConfiguration

It receives `ClientOrder` message sent by Sender and replies to them with `ClientOrderAccepted`.

snippet: Reply


### Multi-instance connection lookup

Both sender and receiver provide a custom lookup mechanism for providing connection information for given destination. The following snippet shows lookup logic used by Sender.

snippet: SenderConnectionProvider


## How it works

Sender and Receiver use [different catalogs](/transports/sql/deployment-options.md) on the same SQL Server instance. The tables representing queues for a particular endpoint are created in the appropriate catalog, i.e. in the `receivercatalog` for the Receiver endpoint and in the `sendercatalog` for the Sender endpoint. It is possible to register a custom `SqlConnection` factory that provides connection instance per given transport address. The operations performed on queues stored in different catalogs are atomic because SQL Server allows multiple `SqlConnection` enlisting in a single distributed transaction.

NOTE: In this sample DTC is required by the Receiver because it operates on two different catalogs when receiving Sender's request. It picks message from input queue stored in `receivercatalog` and sends back reply to Sender's input queue stored in `sendercatalog`. In addition `error` queue is stored also in `sendercatalog` so without DTC Receiver will not be able handle failed messages properly.
