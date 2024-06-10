---
title: Unit of work using custom pipeline behavior
summary: Shows how to use IoC and a pipeline behavior to create a custom unit of work implementation.
reviewed: 2023-08-14
component: Core
related:
 - nservicebus/pipeline
 - nservicebus/pipeline/unit-of-work
---


## Introduction

This sample uses a custom pipeline behavior to manage a unit of work. The unit of work in this sample is a custom session object. In a real application this is often a database session represented by a low-level storage connection/transaction or session, or an ORM session.

Examples are:

- a SQL Server or PostgreSQL connection and/or transaction object 
- an ORM session like Entity Framework DbContext or an NHibernate session
- a storage session provided by a storage SDK client like MongoDB, RavenDB, CosmosDB, etc.

> [!NOTE]
> This is the recommended approach for managing a unit of work as `IManageUnitOfWork` is obsolete in NServiceBus version 9.

Any information from the incoming message headers and body can be used to create or initialize the custom unit of work which is common in multi-tenant or partitioned environments

- Use header/body information to utilize a specific connection string
- Use header/body information to set a query filter on an ORM to only return data for a specific tenant

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

Run the sample. Once running, press any key to send messages. Note that for each given message the two message handlers will get the same session instance.
