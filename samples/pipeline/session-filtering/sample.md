---
title: Session filter pipeline extension
summary: How to extend the pipeline to filter out messages from older sessions.
reviewed: 2019-10-04
component: Core
related:
 - nservicebus/pipeline
---


This sample shows how to extend the NServiceBus message processing pipeline with custom behaviors to add session filters to an endpoint. An endpoint will accept messages only from a sending endpoint if they share a session key.

NOTE: This technique can be useful in testing scenarios where left over messages from previous test runs should be ignored. 


## Code walk-through

The solution contains two endpoints, _Sender_ and _Receiver_, which exchange instances of `SomeMessage`. Each endpoint contains an instance of a session key provider:

snippet: session-key-provider-interface

In the sample, there is a simple implementation that provides a limited set of session keys:

snippet: rotating-session-key-provider

Each endpoint creates an instance of this session key provider and adds it to the endpoint configuration:

snippet: add-filter-behavior

The `ApplySessionFilter` extension method adds two behaviors to the endpoint pipeline:

snippet: config-extension

The first behavior adds the session key as a header to all outgoing messages:

snippet: apply-session-header

The second behavior checks incoming messages for the session key header and only processes messages that have a matching session key:

snippet: filter-incoming-messages

## Running the Code

 * Run the solution.
 * Verify that each endpoint is using the same session key
 * Send some messages from the sender to the receiver
 * Verify that the messages are sent and received correctly
 * Change the session key for the receiver
 * Send more messages from the sender to the receiver
 * Note that the messages are dropped and not processed
 * Change the session key for the sender to match the receiver
 * Send a final batch of messages
 * Verify that the new batch of messages are received