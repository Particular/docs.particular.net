---
title: Dipsatch notification pipeline extension
summary: Extending the pipeline to fire a notification when messages are dispatched.
reviewed: 2017-09-29
component: Core
tags:
 - Pipeline
related:
 - nservicebus/pipeline
---


## Introduction

This sample shows how to extend the NServiceBus message processing pipeline with custom behaviors to add notification whenever a message is dispatched to the underlying transport.


## Code Walk Through

The solution contains a single endpoint with a dispatch notifications turned on. Disptch notifications are handled by a classes that implement a simple interface:

snippet: watch-interface

The sample endpoint contains a dispatch watcher that simply writes the details of dispatch operations to the console:

snippet: sample-dispatch-watcher

An instance of this watcher is added to the endpoint:

snippet: endpoint-configuration 

This enables the underlying feature and adds the watcher to a list which is tracked in the config settings:

snippet: config-extensions

NOTE: Using `EnableByDefault` means that the feature can still be explicitly disabled in code.

The feature (if enabled) is called during the endpoint startup:

snippet: dispatch-notification-feature

The feature injects the watches configured by the user into a new pipeline behavior which sits in the Dispatch Context:

snippet: dispatch-notification-behavior

The behavior notifies all of the watches after the transport operations have been dispatched. 


## Running the Code

 * Run the solution.
 * Press any key other than Escape to send a message
 * As the message is dispatched to the transport, the watch is called which writes the details of the dispatch to the console.