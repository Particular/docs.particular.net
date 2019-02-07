---
title: Implementing cross-cutting concerns using pipeline behaviors
summary: Shows how to use the pipeline to implement cross-cutting concerns such as authentication and authorization
reviewed: 2019-02-07
component: Core
tags:
 - Pipeline
related:
 - nservicebus/pipeline
---


## Introduction

This sample leverages the pipeline to implement support for custom authorization and authentication based on message headers. Each messages is required to carry custom headers that are used to verify if the sender of the message is authorized to trigger a given action. 

Instead of extracting the information in each header separately, this sample uses a pipeline behavior that passes the extracted information to all handlers using the pipeline context extensions.


## Code Walk Through


### Creating the Behavior

The behavior will wrap handler invocations and:

 1. Retrieve values for `auth_login` and `auth_token` headers.
 1. Instantiate an object that wraps the authorization and authentication information.
 1. Store that object in the pipeline context extensions bag.
 1. Pass the control to the handlers.

snippet: auth-behavior


### Registering the behavior

The following code is needed to register the behavior in the receive pipeline.

snippet: configuration


### Message handlers

The handler can access the authorization and authentication information via the extensions bag available on `IMessageHandlingContext`.

snippet: message-handlers


## Running the sample

Run the sample. Once running press any key to send messages.
