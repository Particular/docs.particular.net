---
title: Unit of work using the pipeline
summary: Shows how to use IoC and the pipeline to create a unit of work implementation.
reviewed: 2020-07-16
component: Core
related:
 - nservicebus/pipeline
 - nservicebus/pipeline/unit-of-work
---


## Introduction

This sample leverages the pipeline provided unit of work management for message handlers. Using the pipeline abstraction is necessary when access to the incoming message and/or headers is required.


## Code walk-through


### Creating the Behavior

The behavior will wrap handler invocations and:

 1. Open a session to the database.
 1. Make the session available to the message handlers.
 1. Commit/Rollback the session depending on the outcome of the handler.
 1. Dispose the session.

snippet: unit-of-work-behavior

Note that the injected session factory is responsible for creating the session and that the session is registered in the pipeline context using `context.Extensions.Set<IMySession>(session);`. This will be used later to provide the session to the handlers.


### Registering the behavior

The following code is needed to register the behavior in the receive pipeline.

snippet: configuration


### Providing the session to handlers

While it is possible to use the code `contex.Extensions.Get<IMySession>` in the handler, it is better to provide extension methods on `IMessageHandlerContext` to allow for a more terse syntax. In this sample the methods `.Store<T>` a `.GetSession` are provided:

snippet: session-context-extensions


### Message handlers

One of the benefits of a unit of work is that multiple handlers for the same message will share the same session and commit/rollback together. This is how the handlers look:

snippet: message-handlers


## Running the sample

Run the sample. Once running press any key to send messages. Note that for each given message the two message handlers will get the same session instance.
