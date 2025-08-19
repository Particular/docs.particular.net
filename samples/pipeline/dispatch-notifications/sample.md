---
title: Dispatch notification pipeline extension
summary: Extending the pipeline to fire a notification when messages are dispatched.
reviewed: 2024-07-03
component: Core
related:
 - nservicebus/pipeline
---

## Introduction

This sample shows how to extend the NServiceBus message processing pipeline with custom behavior to add notifications whenever a message is dispatched to the underlying transport.

## Code walk-through

The solution contains a single endpoint with the dispatch notifications turned on. Dispatch notifications are handled by classes that implement the following interface:

snippet: notifier-interface

The sample endpoint contains a dispatch notifier that writes the details of dispatch operations to the console:

snippet: sample-dispatch-notifier

An instance of this notifier is added to the endpoint:

snippet: endpoint-configuration

This enables the underlying feature and adds the notifier to a list which is tracked in the config settings:

snippet: config-extensions

> [!NOTE]
> Using `EnableByDefault` means that the feature can still be explicitly disabled in code.

The feature (if enabled) is called during the endpoint startup:

snippet: dispatch-notification-feature

The feature injects the notifiers configured by the user into a new pipeline behavior which sits in the Dispatch Context:

snippet: dispatch-notification-behavior

The behavior notifies all of the notifiers after the transport operations have been dispatched. For all dispatch operations that failed the notifiers will not be called because the exception would bubble out of the `await next()` call.

## Running the Code

- Run the solution.
- Press any key other than Escape to send a message
- As the message is dispatched to the transport, the registered notifiers are invoked. One writes the details of the dispatch operations to the console.
