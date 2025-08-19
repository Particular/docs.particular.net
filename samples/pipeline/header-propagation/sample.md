---
title: Header propagation using the pipeline
summary: How to use the pipeline to copy a header from incoming messages to outgoing messages
reviewed: 2024-12-10
component: Core
related:
 - nservicebus/pipeline
 - nservicebus/messaging/headers
---

## Introduction

This sample shows how to use the NServiceBus pipeline to copy a header from an incoming message to an outgoing message. This allows information to propagate along a message conversation transparently.

## Code walk-through

### Creating the behavior

The behavior will check for an incoming message with the `CustomerId` header. If one is found, the header value is copied to the outgoing message:

snippet: behavior

### Registering the behavior

The following code registers the behavior in the receive pipeline:

snippet: register-behavior

### Sending a message

The sender sets a custom `CustomerId` header on the message before sending it:

snippet: send-message

### Message handlers

The message handlers can access the custom `CustomerId` header:

snippet: handlers

Note that the second handler is for a message that did not have the `CustomerId` header explicitly set. It was copied from the original message.

## Running the sample

Run the sample, then press any key in the Sender window to send messages. The Receiver will execute two handlers. Note that the Sender and both Receiver handlers have access to the same `CustomerId` header.
