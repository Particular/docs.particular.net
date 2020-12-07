---
title: Injecting tenant-aware components into message handlers
summary: How to configure IoC to inject tenant-aware components into message handlers
reviewed: 2020-12-07
component: Core
related:
- nservicebus/pipeline/manipulate-with-behaviors
---


## Introduction

This sample shows how to configure the dependency injection infrastructure built into NServiceBus to instantiate and inject tenant-aware components. The approach allows the code in the handlers to focus on the actual business logic without having to deal with multi-tenant aspects of the problem.

This sample simulates a multi-tenant solution where the session provided to handlers is connected to individual tenant databases based on the value of a `tenant` header on the incoming message.


## Code walk-through


### Creating the behavior

The behavior will wrap handler invocations and:

 1. Open a session to the relevant tenant database.
 1. Make the session available to the message handlers.
 1. Commit or rollback the session depending on the outcome of the handler.
 1. Dispose the session.

snippet: unit-of-work-behavior

Note that because the user can't provide arguments when resolving the session instance, the session has to be initialized after construction. The `Initialize` method is defined on the concrete `MySession` class. It is not visible to the handler because the handler requires only the `ISession` interface.


### Registering the behavior

The following code is needed to register the behavior in the receive pipeline.

snippet: configuration

NOTE: The lifecycle has to be specified as `DependencyLifecycle.InstancePerUnitOfWork` in order to ensure there is a single instance of session created for each message being handled.


### Message handlers

One of the benefits of a unit of work is that multiple handlers for the same message will share the same session and commit/rollback together. This is how the handlers look:

snippet: message-handlers

The session instance is injected via the constructor. This is the same instance as the one resolved and initialized in the `MyUowBehavior`.


## Running the sample

Run the sample. Once running, press any key to send messages. Note that for each given message the two message handlers will get the same session instance and that the instance is connected to the given tenant specified on the incoming message.
