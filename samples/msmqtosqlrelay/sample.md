---
title: MsmqToSql Relay
summary: This sample shows how to setup a SQL Relay so it can receive events from a MSMQ publisher.
related:
- nservicebus/transports
- nservicebus/msmq
- nservicebus/sqlserver
---

## SQL Relay (Transport Integration)

This sample shows how to setup a SQL Relay so it can receive events from a MSMQ publisher. The solution comprises of these 4 projects.

NOTE: This sample uses NHibernate persistence. It uses a database called, `PersistenceForMsmqTransport` for the MSMQPublisher endpoint.

## Shared

The schema for the events that will be published by `MsmqPublisher`. It contains an event called, `SomethingHappened`

snippet:event


## MsmqPublisher
This is an endpoint which uses MSMQ as the transport and publishes an event.

### The publisher configuration.

snippet:publisher-config

### The publish loop

snippet:publisher-loop


### Additional entry to the list of subscribers

Add a new entry in the Subscriptions table so that when an event is published that message will also be propagated to the specified MSMQ being watched by the `NativeMsmqToSql` endpoint. 

Run this script to add the new entry to the Subscriptions table:

snippet:AddSubscriber
 
## NativeMsmqToSql

Note: Since `NativeMsmqToSql` is using native msmq transport you will need to manually create a transactional `MsmqToSqlRelay` queue.

This endpoint is setup to read messages that arrive in MSMQ natively.

### How does it work

- It uses native .Net Messaging to read messages from the specified Input queue. The input queue is the same queue address that has been added to MSMQ Publisher's subscription list.

snippet:receive-from-msmq-using-native-messaging

- When a message is received, the body and the header of the message is extracted and using Native Sql commands, the information is written to the table. The table's name is the endpoint's name of the SqlRelay endpoint. 

snippet:read-message-and-push-to-sql

- Messages are written to SQL natively using [Sql Scripts](/nservicebus/sqlserver/operations-scripting.md)

## SqlRelay

- This endpoint uses SqlTransport. 
- Since the messages to this endpoint are being written natively by the `NativeMsmqToSql` program, this endpoint is configured to have its auto subscription for events turned off.
- References the `Shared` message schema dll.
- Has a handler for the event that does a publish in the handler, so that downstream SQL subscribers can receive the event.   


### The SqlRelay configuration

snippet:sqlrelay-config


### The event handler

snippet:sqlrelay-handler


## Summary

1. Creating a new transactional queue that the MSMQ publisher will be sending its events to.
2. In the original MSMQ Publisher's Subscriptions storage, in addition to its list of all its current subscribers, add an additional entry for the queue that the MsmqToSqlRelay endpoint will be listening to.
3. The NativeMsmqToSql app will read the message and write to the SQL table of the SqlRelay endpoint.
4. The SqlRelay endpoint receives the message and publishes the event for downstream SQL subscribers 

Note: The steps of creating the queue and adding the additional subscription message in the subscriptions queue of the publisher can be automated for deployment.
