---
title: Msmq-to-SQL Relay
summary: How to set up a SQL relay to receive events from an MSMQ publisher.
reviewed: 2021-01-21
component: MsmqTransport
related:
- transports
- transports/msmq
- transports/sql
redirects:
- samples/msmqtosqlrelay
- samples/msmq/sql-bridge
---

This sample is an example of transport integration, showing how to receive events published by an endpoint whose transport is different from that of the subscriber. Here, events published from an MSMQ endpoint will be relayed to an SQL endpoint for downstream SQL subscribers. The strategy shown can be applied to any transport integration, such as integrating the work of different teams, or crossing the boundary between on-premises and cloud-based systems.

downloadbutton

The solution consists of an endpoint that publishes MSMQ messages, an endpoint that uses the SQL transport, a NativeMsmqToSql program that moves messages from MSMQ to SQL, and a shared assembly.


## Shared

This project contains the schema for the events that will be published by the `MsmqPublisher` endpoint. It contains an event called `SomethingHappened`:

snippet: event


## MsmqPublisher

 * This endpoint uses MSMQ as the transport. It publishes events every time <kbd>enter</kbd> is pressed.
 * This endpoint uses the NHibernate persister. It uses a database called `PersistenceForMsmqTransport` for the persistence. Use the script `CreateDatabase.sql` contained in this project to create the `PersistenceForMsmqTransport` database as well as the tables required for NHibernatePersistence. Alternatively, running the MsmqPublisher within Visual Studio in debug mode will also create the necessary tables for subscription and timeout storage.

snippet: CreateDatabase


### Endpoint configuration for MsmqPublisher.

snippet: publisher-config


### Publishing events

snippet: publisher-loop


### Additional entry to the list of subscribers

For all the events published by the `MsmqPublisher`, add a corresponding new entry in the _Subscriptions_ table so that the `NativeMsmqToSql` endpoint will start to receive these events.

Run the `AddSubscriber.sql` script contained in this project to add a new entry for the `SomethingHappened` event to the _Subscriptions_ table:

snippet: AddSubscriber


## NativeMsmqToSql

`NativeMsmqToSql` is a program that reads messages from a queue using native Messaging MSMQ API and uses the SqlClient API to write information in the corresponding SQL table for the `SqlRelay` endpoint.

Since this is not an NServiceBus endpoint, the required transactional queue named `MsmqToSqlRelay` must be created manually.


### How does it work

When messages arrive in the `MsmqToSqlRelay` queue, they are read using the .NET Messaging API.

snippet: receive-from-msmq-using-native-messaging

When a message arrives at the queue, the body and the header of the message is extracted and the information is stored in the SQL table, `SqlRelay` [using the SQL Client API](/transports/sql/operations-scripting.md).

snippet: read-message-and-push-to-sql


## SqlRelay

This endpoint uses the SQL transport with a database called `PersistenceForSqlTransport`. Use the script `CreateDatabase.sql` contained in this project to create the `PersistenceForSqlTransport` database.

snippet: CreateDatabaseForSqlPersistence

 * This endpoint references the `Shared` message schema.
 * Messages to this endpoint are written natively by the `NativeMsmqToSql` program. Since no SQL endpoint is publishing the events, this endpoint is configured to have automatic subscription for events turned off.
 * This endpoint has a handler for the `SomethingHappened` event, which in turn does a publish within that handler so that downstream SQL subscribers can receive the event.


### The SqlRelay configuration

snippet: sqlrelay-config


### The event handler

snippet: sqlrelay-handler


## Summary

 1. Create a new transactional queue that the MSMQ publisher will send its events to in addition to its current subscribers.
 1. For all events published by the `MsmqPublisher` add a corresponding new entry in the _Subscriptions_ table.
 1. The NativeMsmqToSql app will read the messages that arrive at this new transactional queue and write the corresponding message information in the SQL table of the SqlRelay endpoint.
 1. The SqlRelay endpoint receives the message and publishes the event for downstream SQL subscribers.

Note: The deployment steps can be automated to create the required queues and tables.
