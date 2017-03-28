---
title: Non Durable messaging on MSMQ
summary: Sending non-durable message by using the Express Attribute.
reviewed: 2016-03-21
component: Core
related:
 - nservicebus/messaging/non-durable-messaging
 - nservicebus/msmq/connection-strings
---

## Code walk-through

This sample shows the use the [Non-Durable Messaging](/nservicebus/messaging/non-durable-messaging.md) feature of NServiceBus to send non-durable messages via MSMQ.

The project contains three projects:

 * Sender: Sends the express message
 * Receiver: receives the express message
 * Shared: Common functionality shared between both endpoints including the message definition.


## Required actions for non-durable messages to function

The are several configuration options that must be set for express message to


### Non-transactional Endpoints

The endpoints must be configured to be non-transactional.

snippet: non-transactional


### MSMQ configured to be non-transactional

To ensure MSMQ is used in a setup and used non-durable way the `NServiceBus/Transport` connection string must contain `useTransactionalQueues=false`.

snippet: useTransactionalQueues-false


### Express Attribute

Messages must have an `[ExpressAttribute]`.

snippet: message-definition


## Running the solution

Run the solution with both projects as startup project and not the output that the handler in Receiver successfully processes the message


### Look at the queue

View the properties of the queues and notice they set to be non-transactional. This is required for non-durable messaging in MSMQ since only non-transactional queues support [Express Mode](https://msdn.microsoft.com/en-us/library/ms704130).


### Viewing the message

Start the `Sender` project on it own it will send a message to the `Samples.MessageDurability.Receiver` queue. Open this message and notice the `Recoverable` property is `false`.