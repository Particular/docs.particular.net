---
title: Multi-Instance Mode
summary: SQL Server transport running in multi-instance mode using Bridge
reviewed: 2023-07-04
component: Bridge
related:
- nservicebus/bridge
- transports/sql
- transports/sql/deployment-options
---

This is sample shows how to use the [NServiceBus Messaging Bridge](/nservicebus/bridge/) instead of the deprecated [SQL Server transport multi-instance mode](/transports/upgrades/sqlserver-31to4.md#multi-instance-mode).

## Prerequisites

include: sql-prereq

The databases created by this sample are `NsbSamplesSqlMultiInstanceReceiver` and `NsbSamplesSqlMultiInstanceSender`.

## Running the project

 1. Start the following projects:
    1. Sender
    1. Receiver
    1. Bridge

 1. Press <kbd>enter</kbd> in the Sender's console window to send a new message.

## Verifying that the sample works correctly

 1. The Receiver displays information that an order was submitted.
 1. The Sender displays information that a response arrived for the same order.

## Code walk-through

The sample contains the following projects:

* Sender: A console application responsible for sending the initial `ClientOrder` message and processing the follow-up `ClientOrderResponse` message.
* Receiver: A console application responsible for processing the order message.
* Bridge: A console application responsible for routing messages across the two database instances.
* Messages: A class library containing message definitions.
* Helpers: A class library for creating the databases and schemas.

### Sender project

The sender does not store any data. It mimics a front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use the SQL Server transport. Other than that, it is unaware the other endpoint is running on a different database instance.

snippet: SenderConfiguration

The sender sends a message to the receiver:

snippet: SendMessage

### Receiver project

The receiver mimics a back-end system. It is configured to use the SQL Server transport but has a different connection string from the sender.

snippet: ReceiverConfiguration

Note that the endpoint configuration contains no routing information, as the response message is a regular reply and NServiceBus, together with the bridge, will take care of all the routing with reply messages. The receiver replies with `ClientOrderResponse` back to the sender.

snippet: Reply

### Bridge

The bridge is configured with two transports. As both transports are of the same `SqlServerTransport` type, it is required to provide a name to each `BridgeTransport` to distinguish between the two in log files.

snippet: BridgeConfiguration

Both transports have the endpoints defined on their side and as a result, the bridge will mimic those endpoints on the other side. This way it becomes transparent to actual endpoints on either side that those endpoints are actually bridged.
