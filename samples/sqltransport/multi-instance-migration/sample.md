---
title: Multi-Instance Mode to Bridge migration
summary: Migration of SQL Server transport Multi-Instance Mode topology to Bridge
reviewed: 2017-09-04
component: SqlTransport
related:
 - transports/sql/deployment-options
---


## Prerequisites

include: sql-prereq

The databases created by this sample are `NsbSamplesSqlMultiInstanceReceiver3`, `NsbSamplesSqlMultiInstanceSender3`, `NsbSamplesSqlMultiInstanceReceiver4`, `NsbSamplesSqlMultiInstanceSender3` and `NsbSamplesSqlMultiInstanceBridge`.

Ensure [Distributed Transaction Coordinator (DTC)](https://msdn.microsoft.com/en-us/library/ms684146.aspx) is running. It can be started from the command line by running `net start msdtc`.


## Running the project

 1. Start all projects.
 1. Hit enter in Sender V3 and V4 consoles window to send messages.


## Verifying that the sample works correctly

 1. The Receiver displays information that an order was submitted.
 1. The Sender displays information that the order was accepted.


## Code walk-through

This sample contains the following projects:

 * Sender.V3 - A console application responsible for sending the initial `ClientOrder` message and processing the follow-up `ClientOrderAccepted` message.
 * Receiver.V3 - A console application responsible for processing the order message.
 * Sender.V4 - A sender application upgraded to Version 7 of NServiceBus and Version 4 of SQL Server transport.
 * Receiver.V4 - A receiver application upgraded to Version 7 of NServiceBus and Version 4 of SQL Server transport.
 * Shared - A class library containing message definitions.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. In Version 3 it is configured to use SQL Server transport and run in the [*multi-instance*](/transports/sql/deployment-options.md#modes-overview-multi-instance) mode. `ConnectionProvider.GetConnection` method is used for providing connections.

snippet: SenderConfigurationV3

The Sender sends a message to the Receiver:

snippet: SendMessage

In Version 4 the sender is configured to route the `ClientOrder` messages through a bridge:

snippet: SenderConfigurationV4


### Receiver project

The Receiver mimics a back-end system. In Version 3 it is configured to use SQL Server transport in the [*multi-instance*](/transports/sql/deployment-options.md#modes-overview-multi-instance) mode.

snippet: ReceiverConfigurationV3

It receives `ClientOrder` message sent by Sender and replies to them with `ClientOrderAccepted`.

snippet: Reply

In Version 4 the receiver does not requires any special configuration. The reply is sent back to the bridge:

snippet: ReceiverConfigurationV4


### Bridge

The bridge application runs the [Transport Bridge](/nservicebus/bridge/) component:

snippet: BridgeConfiguration

The bridge is configured to use SQL Server transport on both sides and to use [SQL Persistence](/persistence/sql/)-based subscription storage for Pub/Sub. It uses the default TransactionScope [transport transaction mode](/transports/sql/transactions) to ensure that *exactly-once* semantics are preserved when messages traverse the bridge.


## How it works

In Version 3 both endpoints have to know each other's connection strings. When sending a message a connection string is selected based on the destination queue address. Because the messages don't contain information about the origin's queue connection string, the receiver has to know the connection string to use when sending a reply. This creates unnecessary physical coupling back to the sender.

In Version 4 the Transport Bridge mediates between the sender and the receiver. Based on sender's bridge connector configuration the transport sends the message not to the designated endpoint but to the bridge. The bridge forwards it to the destination, replacing the reply-to address. Because of this the receiver does not have to know if sender is behind the bridge or not. The reply is routed back to the bridge and forwarded to the originator without requiring any specific receiver-side configuration.
