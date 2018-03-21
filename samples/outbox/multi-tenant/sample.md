---
title: Multi-tenant endpoints
summary: Configure persistence to support multi-tenant scenarios with database-per-tenant isolation.
reviewed: 2018-01-29
component: Core
tags:
- Outbox
related:
- persistence/nhibernate
---

This sample demonstrates how to create a multi-tenant environment, where both user data and NSeriviceBus data for multiple tenants (i.e. clients/customers) are each stored in a dedicated database for that tenant. In this sample, it's not necessary to have complete separation of all NServiceBus endpoints, and adding a new tenant does not necessitate provisioning an entirely new set of NServiceBus endpoints devoted to the new tenant.

Instead, the `TenantId` is propagated by the NServiceBus infrastructure as custom message header. Extensions to the NServiceBus message-processing pipeline ensure that a connection to the correct database is opened based on the incoming `TenantId`, and that the `TenantId` from a message being processed is propagated to any outgoing messages created by the message handler.

downloadbutton


## Prerequisites

include: sql-prereq

The databases created by this sample are:

 * `NsbSamplesMultiTenantSender`
 * `NsbSamplesMultiTenantReceiver`
 * `NsbSamplesMultiTenantA`
 * `NsbSamplesMultiTenantB`


## Running the project

 1. Start the Sender project (right-click on the project, select the `Debug > Start new instance` option).
 1. The text `Press <enter> to send a message` should be displayed in the Sender's console window.
 1. Start the Receiver project (right-click on the project, select the `Debug > Start new instance` option).
 1. The Sender should display subscription confirmation `Subscribe from Receiver on message type OrderSubmitted`.
 1. Press `A` or `B` on the Sender console to send a new message either to one of the tenants.


## Verifying that the sample works correctly

 1. The Receiver displays information that an order was submitted.
 1. The Sender displays information that the order was accepted.
 1. Finally, after a couple of seconds, the Receiver displays confirmation that the timeout message has been received.
 1. Open SQL Server Management Studio and go to the tenant databases. Verify that there are rows in saga state table (`dbo.OrderLifecycleSagaData`) and in the orders table (`dbo.Orders`) for each message sent.

WARNING: Timeouts are stored in the shared `Receiver` database so make sure to not include any sensitive information in the timeouts. Keep such information in the saga data and only use timeouts as notifications.


## Code walk-through

This sample contains three projects:

 * Shared - A class library containing common code including messages definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the `OrderSubmitted` message, sending `OrderAccepted` message and randomly generating exceptions.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use MSMQ transport with NHibernate persistence and Outbox.


### Receiver project

The Receiver mimics a back-end system. It is also configured to use MSMQ transport with NHibernate persistence and Outbox.


#### Creating the schema

snippet: ReceiverConfiguration

snippet: CreateSchema

The above code makes sure that user, saga and outbox tables are created in the tenant databases while the timeouts and subscriptions -- in the shared database.


#### Tenant database

To allow for database isolation between the tenants the connection to the database needs to be created based on the message being processed. This requires cooperation of several components:

 * Custom `ConnectionProvider` for NHibernate
 * A behavior that injects an incoming message and opens a new connection based on the tenant ID found in the headers of that message
partial: propagatetenantidoutgoing


#### Connection provider

The custom connection provider has to be registered with NHibernate

snippet: ConnectionProvider

partial: CapturePipelineExecutor

The connection provider looks at the current message processing context. If there is an existing connection to the tenant database, it creates a new one with the same connection string. Otherwise, the connection provider defaults to creating a connection to the shared database.

snippet: GetConnectionFromContext

NOTE: The connection provider is only used by `OutboxPersister`'s `TryGet` and `MarkAsDispatched` methods which execute in separate transaction from all the other storage operations.

NOTE: The connection provider is a simple implementation that is not thread-safe. For this example, the Maximum Concurrency Level is set to 1 which makes it run in single thread mode.

#### Opening connection to tenant database

The `MultiTenantOpenSqlConnectionBehavior` behavior extracts the `TenantId` header from the incoming message and looks up the matching connection string in the app.config file.

snippet: OpenTenantDatabaseConnection

This behavior needs to replace the built-in behavior.

snippet: ReplaceOpenSqlConnection


#### Propagating the tenant information downstream

Finally, the `PropagateTenantIdBehavior` behavior makes sure that tenant information is not lost and that all outgoing messages have the same tenant ID as the message being processed.

snippet: PropagateTenantId

This behavior also needs to be registered a configuration time.

snippet: RegisterPropagateTenantIdBehavior
