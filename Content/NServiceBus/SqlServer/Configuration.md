---
title: Queue table design in SQLServer transport
summary: Design of tables that are used as queues in thr SQLServer transport
tags:
- SQLServer
- Transport
---

## Queue table structure

Following SQL DDL is used to create a table for a queue:

```SQL
CREATE TABLE [dbo].[{0}](
	[Id] [uniqueidentifier] NOT NULL,
	[CorrelationId] [varchar](255) NULL,
	[ReplyToAddress] [varchar](255) NULL,
	[Recoverable] [bit] NOT NULL,
	[Expires] [datetime] NULL,
	[Headers] [varchar](max) NOT NULL,
	[Body] [varbinary](max) NULL,
	[RowVersion] [bigint] IDENTITY(1,1) NOT NULL
) ON [PRIMARY];
```

Additionally, a clustered index on a ```[RowVersion]``` column is created. The column are directly mapped to the properties of ```NServiceBus.TransportMessage``` class. Receiving messages is conducted via a ```DELETE``` statement from the top of the table (the oldest row according to ```[RowVersion]``` column).

## Concurrency

The SQLServer transport starts up to ```MaximumConcurrencyLevel``` ([set via ```TransportConfig``` section](msmqtransportconfig.md) threads, each running the receive loop in which it tries to receive a single message via aforementioned ```DELETE``` statement and, if succeeded, passes that message into the processing pipeline. Otherwise it backs up for a while.

## Transactions

## Primary queue

For each endpoint there is a single primary queue table which name matches the name of the endpoint. This single queue is shared by all instances in case of a scale-out scenario.

## Secondary queues

In order for callbacks (registerd via ```ICallback``` interface returned by ```IBus.Send```) to work in a scale-out scenario each endpoint instance has to have its own queue/table. This is ncessary because callback handlers are stored in-memory in the node that did the send. The reply (via ```Bus.Reply```) should be delivered to this special queue so that it is picked up by the same node that registered the callback.

Secondary queue tables have the name of the machine appended to the name of the primary queue table with ```.``` as separator e.g. ```SomeEndpoint.MyMachine```.

Secondary queues are enabled by default. In order to disable them, one must use the configuration API:

```C#
busConfiguration.UseTransport<SqlServerTransport>().DisableCallbackReceiver();
```

Secondary queues use same concurrency model to the primary queue but use different settings for the maximum concurrency level. The default value of this setting is 1 which schould be fine with most scenarios because it is small enough to not degrade the overall performance of the endpoint and large enough to cope with usually small number of callbacks. In order to change this value one should use the configuration API:

```C#
busConfiguration.UseTransport<SqlServerTransport>().CallbackReceiverMaxConcurrency(8);
```

### Satellites

Secondary queues (and hence callbacks) are disabled for satellite receivers.
