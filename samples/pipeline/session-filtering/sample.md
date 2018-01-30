---
title: Session filter pipeline extension
summary: Extending the pipeline to filter out messages from older sessions.
reviewed: 2017-12-18
component: Core
tags:
 - Pipeline
related:
 - nservicebus/pipeline
---


## Introduction

This sample shows how to extend the NServiceBus message processing pipeline with custom behaviors to add session filters to an endpoint. An endpoint will only accept messages from a sending endpoint if they share a session key.

NOTE: This technique can be useful in testing scenarios where left over messages from previous test runs should be ignored. 


## Code Walk Through

The solution contains two endpoints _Sender_ and _Receiver_ which exchange `SomeMessage` instances. Each endpoint contains an instance of a session key provider:

snippet: session-key-provider-interface

In the case of the sample, there is a simple implementation that provides a limited set of session keys:

snippet: rotating-session-key-provider

Each endpoint creates an instance of this session key provider and adds it to the endpoint configuration:

snippet: add-filter-behavior

The `ApplySessionFilter` extension method adds two behaviors to the endpoint pipeline:

snippet: config-extension

The first behavior adds the session key as a header to all outgoing messages:

snippet: apply-session-header

The second behavior checks incoming messages for the session key header and only processes messages with a matching session key:

snippet: filter-incoming-messages

## Running the Code

 * Run the solution.
 * Verify that each endpoint is using the same session key
 * Send some messages from the sender to the receiver
 * Verify that the messages are being sent and received correctly
 * Change the session key for the receiver
 * Send some more messages from the sender to the receiver
 * Note that the messages are being dropped and not processed
 * Change the session key for the sender to match the receiver
 * Send a final batch of messages
 * Verify that the new batch of messages are being received