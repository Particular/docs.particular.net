---
title: MsmqToSql Relay
summary: Setup a SQL Relay so it can receive events from a MSMQ publisher.
reviewed: 2017-06-21
component: Core
related:
- transports
- transports/msmq
- transports/sql
redirects:
- samples/msmqtosqlrelay
---

This sample is an example of transport integration, showing how to receive events published by an endpoint whose transport is different from that of the subscriber. In this particular example, events published from an MSMQ endpoint will be relayed to an SQL endpoint for downstream SQL subscribers. The strategy shown can be applied to any transport integration, such as integrating the work of different teams, or crossing the boundary between on-premises and cloud-based systems.

downloadbutton

The solution consists of an endpoint that publishes MSMQ messages, an endpoint that uses the SQL Transport, a NativeMsmqToSql program that moves messages from MSMQ to SQL, and a Shared assembly.


## Shared

This project contains the schema for the events that will be published by `MsmqPublisher` endpoint. It contains an event called `SomethingHappened`:

snippet: event


## MsmqPublisher

 * This endpoint uses MSMQ as the transport. It publishes events every time `Enter` key is pressed.
 * This endpoint uses NHibernate persistence. It uses a database called, `PersistenceForMsmqTransport` for the persistence. Use the script `CreateDatabase.sql` contained in this project to create the `PersistenceForMsmqTransport` database and the tables required for NHibernatePersistence. Alternatively running the MsmqPublisher within Visual Studio in debug mode will also create the necessary tables for Subscription and Timeout storage.

snippet: CreateDatabase


### Endpoint configuration for MsmqPublisher.

snippet: publisher-config


### Publishing events

snippet: publisher-loop


### Additional entry to the list of subscribers

For all the events published by the `MsmqPublisher` add a corresponding new entry in the Subscriptions table so that the `NativeMsmqToSql` endpoint will start to receive these events.

Run the `AddSubscriber.sql` script contained in this project to add a new entry for the `SomethingHappened` event to the Subscriptions table:

snippet: AddSubscriber


## NativeMsmqToSql

`NativeMsmqToSql` is a program that reads messages from a queue using native Messaging MSMQ API and uses SqlClient API to write information in the corresponding SQL table for the `SqlRelay` endpoint.

Since this is not an NServiceBus endpoint, create the required transactional queue named `MsmqToSqlRelay`.


### How does it work

When messages arrive in the `MsmqToSqlRelay` queue, they are read using .NET Messaging API.

snippet: receive-from-msmq-using-native-messaging

When a message arrives at the queue, the body and the header of the message is extracted and [using SQL Client API](/transports/sql/operations-scripting.md), the information is stored in the SQL table, `SqlRelay`. 

snippet: read-message-and-push-to-sql


## SqlRelay

 * This endpoint uses SQL transport. It uses a database called, `PersistenceForSqlTransport` Use the script `CreateDatabase.sql` contained in this project to create the `PersistenceForSqlTransport` database.

snippet: CreateDatabaseForSqlPersistence

 * References the `Shared` message schema.
 * Messages to this endpoint are being written natively by the `NativeMsmqToSql` program. Since no SQL endpoint is publishing the events,  this endpoint is configured to have its auto subscription for events turned off.
 * Has a handler for the event that does a publish in the handler, so that downstream SQL subscribers can receive the event.


### The SqlRelay configuration

snippet: sqlrelay-config


### The event handler

snippet: sqlrelay-handler


## Summary

 1. Create a new transactional queue that the MSMQ publisher will be sending its events to in addition to its current subscribers.
 1. For all the events published by the `MsmqPublisher` add a corresponding new entry in the Subscriptions table.
 1. The NativeMsmqToSql app will read the messages that arrive at this new transactional queue and write the corresponding message information in the SQL table of the SqlRelay endpoint.
 1. The SqlRelay endpoint receives the message and publishes the event for downstream SQL subscribers.

Note: The deployment steps can be automated to create the needed queues and tables.
