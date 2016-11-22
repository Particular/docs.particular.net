---
title: Unit of work management using the pipeline
summary: Shows how to use IoC and the pipeline to create a unit of work implementation.
reviewed: 2016-03-21
component: Core
tags:
- Pipeline
- Unit of work
related:
- nservicebus/pipeline
- nservicebus/pipeline/unit-of-work
---


## Introduction

This sample leverages the pipeline provide unit of work management for message handlers. Using the pipeline instead of the [`IManageUnitsOfWork`](/nservicebus/pipeline/unit-of-work.md#implementing-custom-units-of-work-imanageunitsofwork) abstraction is needed when access to the incoming message and/or header is needed.

This sample simulates a multi-tennant solution where the session priovide to handlers is connects to individual tennant databases based on the value of a `tennant` header on the incoming message.

## Code Walk Through

### Creating the Behavior

The behavior will wrap handler invokactions and:

1. Open a session to the relevant tennant database
2. Make the session available to the message handlers
3. Commit/Rollback the session depending on the outcome of the handler
4. Dispose the session

snippet: unit-of-work-behavior

Note how we inject a session factory that is responsible for creating the session and that we register the session is the pipeline context using `context.Extensions.Set<IMySession>(session);`. This will be used later to provide the session to the handlers.

### Registering the behavior and session factory

The following registration code is needed to register the session factory and the behavior.

snippet: configuration

### Providing the session from handlers

While we could write `contex.Extensions.Get<IMySession>` in our handler code it better to provide extension methods on `IMessageHandlerContext` to allow for a more terse syntax. In this sample we provide a `.Store<T>` and a `.GetSession` method using the following:

snippet: session-context-extensions

### Message handlers

One of the benefits of a unit of work is that multiple handlers for the same message will share the same session and commit/rollback together. This is how the handlers look:

snippet: message-handlers

## Running the sample

Just hit F5 to run the sample. Once running press any key to send a few messages. Note that for each given message the two message handlers will get the same session instance and that the instance is connected to the given tennant specified on the incoming message.