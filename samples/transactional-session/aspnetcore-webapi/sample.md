---
title: Using TransactionalSession with Entity Framework and ASP.NET Core
summary: Transactional Session sample that illustrates how to send messages and modify data with Entity Framework in an atomic manner using ASP.NET Core.
component: TransactionalSession.SqlPersistence
reviewed: 2022-08-12
related:
- nservicebus/transactional-session
- nservicebus/transactional-session/persistences/sql-persistence
- persistence/sql
---

include: webhost-warning

This sample shows how to send messages and modify data in a database atomically within the scope of a web request using the `NServiceBus.TransactionalSession` package in conjunction with ASP.NET Core. The operations are triggered by an incoming HTTP request to ASP.NET Core which will manage the `ITransactionalSession` lifetime inside a request middleware.

## Prerequisites

Ensure an instance of SQL Server (Version 2012 or above) is installed and accessible on `localhost` and port `1433`.

Alternatively, change the connection string to point to different SQL Server instance.

At startup each endpoint will create the required SQL assets including databases, tables, and schemas.

## Running the solution

When the solution is run, a new browser window/tab opens, as well as a console application. The browser will navigate to `http://localhost:58118/`.

An async [WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) controller handles the request. It stores a new document using Entity Framework and sends an NServiceBus message to the endpoint hosted in the console application.

The message will be processed by the NServiceBus message handler and result in `"Message received at endpoint"`-message printed to the console. In addition, the handler will update the previously created entity.

## Configuration

The endpoint is configured using the `UseNServiceBus` extension method:

snippet: txsession-nsb-configuration

The transactional session is enabled via the `endpointConfiguration.EnableTransactionalSession()` method call. Note that the transactional session feature requires [the outbox](/nservicebus/outbox/) to be configured in order to ensure that operations across the storage and the message broker are atomic.

ASP.NET Core uses `ConfigureWebHostDefaults` for configuration and a custom middleware is registered for the `ITransactionalSession` lifetime management:

snippet: txsession-web-configuration

Entity Framework support is configured by registering the `DbContext`:

snippet: txsession-ef-configuration

The registration ensures that the `MyDataContext` type is built using the same session and transaction that is used by the `ITransactionalSession`. Once the transactional session is committed, it notifies the Entity Framework context to call `SaveChangesAsync`.

## Using the session

The message session is injected into `SendMessageController` via constructor injection along with the Entity Framework database context. Message operations executed on the `ITransactionalSession` API are transactionally consistent with the database operations performed on the `MyDataContext`.

snippet: txsession-controller

The lifecycle of the session is managed by the `MessageSessionMiddleware`. It opens the session when an HTTP request arrives and takes care of committing the session once the ASP.NET pipeline completes:

snippet: txsession-middleware

This diagram visualizes the interaction between the middleware, `ITransactionalSession`, and the Web API controller:

```mermaid
sequenceDiagram
    autonumber
    User->>Middleware: Http Request
    activate Middleware
    Middleware->>TransactionalSession: Open
    activate TransactionalSession
    TransactionalSession-->>Middleware: Reply
    Middleware->>Controller: next()
    activate Controller
    Controller->>TransactionalSession: Send/Publish...
    Controller->>TransactionalSession: Use SynchronizedStorageSession
    deactivate Controller
    Middleware->>TransactionalSession: Commit
    deactivate TransactionalSession
    Middleware-->>User: Reply
    deactivate Middleware
```

## Handling the message

The `MyHandler` handles the message sent by the ASP.NET controller and accesses the previously committed data stored by the controller:

snippet: txsession-handler
