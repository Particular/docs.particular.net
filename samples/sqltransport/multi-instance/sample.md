---
title: SQL Server Transport Multi-Instance Mode
summary: SqlServer transport running in Multi-Instance Mode.
reviewed: 2016-03-25
tags:
- SQL Server
- Multi-Instance Mode
related:
- nservicebus/sqlserver/deployment-options
---

## Prerequisites

 1. Make sure SQL Server Express is installed and accessible as `.\SQLEXPRESS`.
 1. Create two databases called `receivercatalog` and `sendercatalog`.
 1. Make sure that Distributed Transaction Coordinator (DTC) is running. It can be started running `net start msdtc` command in system console.  


## Running the project

 1. Start the Sender project (right-click on the project, select the `Debug > Start new instance` option).
 1. The text `Press <enter> to send a message` should be displayed in the Sender's console window.
 1. Start the Receiver project (right-click on the project, select the `Debug > Start new instance` option).
 1. Hit `<enter>` in Sender's console window to send a new message.


## Verifying that the sample works correctly

 1. The Receiver displays information that an order was submitted.
 1. The Sender displays information that the order was accepted.


## Code walk-through

This sample contains three projects:

 * EndpointConnectionLookup - A class library containing common code for providing new `SqlConnection` for transport addresses used in the sample.
 * Sender - A console application responsible for sending the initial `ClientOrder` message and processing the follow-up `ClientOrderAccepted` message.
 * Receiver - A console application responsible for processing the order message.
 * Messages - A class library containing message definitions

### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use SQL Server transport and run in `LegacyMultiInstanceMode`. `ConnectionProvider.GetConnection` method is used for providing `SqlConnecion` for each transport address used by Sender endpoint.

snippet:SenderConfiguration

The Sender uses a configuration file to tell NServiceBus where the messages addressed to the Receiver should be sent:

snippet:SenderMessageMappings


### Receiver project

The Receiver mimics a back-end system. It is also configured to use SQLServer transport in `LegacyMultiInstanceMode`.

snippet:ReceiverConfiguration

It receives `ClientOrder` message sent by Sender and replies to them with `ClientOrderAccepted`.

snippet:Reply


### EndpointConnectionLookup project

The EndpointConnectionLookup plays are role or providing `SqlConnection` per each transport address requested either by Sender or Receiver. For every address is returns opened connection string to `sendercatalog` or `receivercatalog`.

snippet:ConnectionProvider


## How it works

Sender and Receiver use [different catalogs](/nservicebus/sqlserver/deployment-options.md) on the same SQL Server instance. The tables representing queues for a particular endpoint are created in the appropriate catalog, i.e. in the `receivercatalog` for the Receiver endpoint and in the `sendercatalog` for the Sender endpoint. `LegacyMultiInstanceMode` enables registering custom `SqlConnection` factory that provides connection instance per given transport address. The operations performed on queues stored in different catalogs are atomic because SQL Server allows multiple `SqlConnection` enlisting in a single distributed transaction.

NOTE: In this sample DTC is required by the Receiver because it operates on two different catalogs when receiving Sender's request. It picks message from input queue stored in `receivercatalog` and sends back reply to Sender's input queue stored in `sendercatalog`. In addition `error` queue is stored also in `sendercatalog` so without DTC Receiver will not be able handle failed messages properly.