---
title: Non-durable messaging on MSMQ
summary: Sending a non-durable message by using the Express Attribute.
reviewed: 2019-02-04
component: MsmqTransport
related:
 - nservicebus/messaging/non-durable-messaging
 - transports/msmq/connection-strings
---

## Code walk-through

This sample shows the use the [Non-Durable Messaging](/nservicebus/messaging/non-durable-messaging.md) feature of NServiceBus to send non-durable messages via MSMQ.

The project contains three projects:

 * Sender: Sends the express message
 * Receiver: receives the express message
 * Shared: Common functionality shared between both endpoints including the message definition.

## Required actions for non-durable messages to function

The are several configuration options that must be set for an express message to

### Non-transactional Endpoints

The endpoints must be configured to be non-transactional.

snippet: non-transactional

### MSMQ configured to be non-transactional

To ensure MSMQ is used the non-durable way the `NServiceBus/Transport` connection string must contain `useTransactionalQueues=false`. Alternatively, transactional queues can be disabled with the following code API.

snippet: useTransactionalQueues-false

### Express Attribute

Messages must have an `[ExpressAttribute]`.

snippet: message-definition

## Running the solution

To run the application, set both projects as startup and note the output that the handler in Receiver successfully processes the message. 

Note: The first time the application runs, it will automatically create the queues for you, but since the reciever queue need to exist first, if you encounter an error saying 'The destination queue could not be found', just re-run the application. 

### Look at the queue

View the properties of the queues and notice they set to be non-transactional. This is required for non-durable messaging in MSMQ since only non-transactional queues support [Express Mode](https://msdn.microsoft.com/en-us/library/ms704130).

### Viewing the message

Start the `Sender` project on its own and it will send a message to the `Samples.MessageDurability.Receiver` queue.
