---
title: SqlTransportNative
summary: Integrating natively with the SQL Server transport.
reviewed: 2018-04-29
component: SqlTransportNative
related:
- transports/sql
---

This sample demonstrates using [SQL Transport - Native](/transports/sql/sql-native.md) to send and manage messages when running on the [SqlServer Transport](/transports/sql).


## Prerequisites

include: sql-prereq

The database created by this sample is `NsbSamplesSqlNative`.


## Running the sample

When the solution is started four endpoints will start up:

 * NsbEndpoint
 * NativeEndpoint
 * AuditConsumer
 * ErrorProcessor

On NativeEndpoint press:

 * `s` to send a message that will succeed or
 * `f` to send a message that will fail 

The successful message will be received by NsbEndpoint and AuditConsumer. The failure message will be be received by NsbEndpoint and ErrorProcessor.


## Code walk-through


### NsbEndpoint

This is a standard NServiceBus endpoint.


#### EndpointConfiguration

The endpoint is configured as follows:

 * Use the [SQL Server Transport](/transports/sql).
 * Use the [Newtonsoft JSON serializer](/nservicebus/serialization/newtonsoft.md). The choice of serializer is important since that format will need to be consistent when sending/receiving in the native context.
 * Forward failed messages to a queue named `error`.
 * Forward successful messages to a queue named `audit`.

snippet: EndpointConfiguration


#### Handler

The handler receives a message and replies with another.

snippet: handler 


#### Message contract

There are two message contracts. One for sending, and one for replying. 

snippet: MessageContract

Note that the messages exist only in this endpoint and do not need to be used, via a reference, in any of the other projects.


### NativeEndpoint

This project uses the [NServiceBus.SqlServer.Native NuGet Package](https://www.nuget.org/packages/NServiceBus.SqlServer.Native/) to send and receive messages.


#### Sending Messages

Messages are sent to NsbEndpoint using `NServiceBus.Transport.SqlServerNative.Sender`.

snippet: sendMessage


##### Message that will succeed

This message will be successfully processed by NsbEndpoint. NsbEndpoint will then send a reply back to NativeEndpoint and a copy of the sent message will be forwarded to the `audit` queue and hence AuditConsumer.

snippet: MessageThatWillSucceed


##### Message that will fail

This message will fail to be processed by NsbEndpoint and be sent to the `error` queue and hence ErrorProcessor.

snippet: MessageThatWillFail


#### Receiving Messages

NativeEndpoint received the reply message from NsbEndpoint using `NServiceBus.Transport.SqlServerNative.MessageProcessingLoop`:

snippet: receive


### AuditConsumer

The `audit` queue is consumed using `NServiceBus.Transport.SqlServerNative.MessageConsumingLoop`

snippet: MessageConsumingLoop


### ErrorProcessor

The `error` queue is processed using `NServiceBus.Transport.SqlServerNative.MessageProcessingLoop`:

snippet: MessageProcessingLoop

Since in this scenario, messages are not deleted as they are being processed, it is necessary to keep track of the last processed `RowVersion`. This is done using `NServiceBus.Transport.SqlServerNative.RowVersionTracker`, which stores the `RowVersion` in a single row table named `RowVersionTracker`.