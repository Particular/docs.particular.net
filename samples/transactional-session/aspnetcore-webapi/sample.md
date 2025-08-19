---
title: Using TransactionalSession with Entity Framework and ASP.NET Core
summary: Transactional Session sample that illustrates how to send messages and modify data with Entity Framework in an atomic manner using ASP.NET Core.
component: TransactionalSession.SqlPersistence
reviewed: 2025-06-04
related:
- nservicebus/transactional-session
- nservicebus/transactional-session/persistences/sql-persistence
- persistence/sql
---

include: webhost-warning

This sample shows how to send messages and modify data in a database atomically within the scope of a web request using the `NServiceBus.TransactionalSession` package with ASP.NET Core. The ASP.NET Core application hosts a [send-only endpoint](/nservicebus/hosting/#self-hosting-send-only-hosting). The operations are triggered by an incoming HTTP request to ASP.NET Core that will manage the `ITransactionalSession` lifetime using a request middleware.

> [!NOTE]
> Starting in version 8.2.0, `NServiceBus.Persistence.Sql.TransactionalSession` is supported in send-only endpoints. Refer to the [documentation](/nservicebus/transactional-session/#remote-processor) for more details.

## Prerequisites

Ensure an instance of SQL Server (Version 2012 or above) is installed and accessible on `localhost` and port `1433`.

Alternatively, change the connection string to point to a different SQL Server instance.

At startup, each endpoint will create the required SQL assets including databases, tables, and schemas.

## Running the solution

When the solution is run, a new browser window/tab opens, as well as a console application. The browser will navigate to `http://localhost:58118/`.

An async [WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) controller handles the request. It stores a new document using Entity Framework and sends an NServiceBus message to the endpoint hosted in the console application.

The message is processed by the NServiceBus message handler in the `Sample.Receiver` project and results in `"Message received at endpoint"` printed to the console. In addition, the handler will update the previously created entity.

To query all the stored entities, navigate to `http://localhost:58118/all`. To apply a complex object hierarchy using the transactional session on an endpoint, navigate to `http://localhost:58118/service`.

## Configuration

The endpoint is configured using the `UseNServiceBus` extension method:

snippet: txsession-nsb-configuration

The transactional session is enabled via the `endpointConfiguration.EnableTransactionalSession()` method call. Note that the transactional session feature requires [the outbox](/nservicebus/outbox/) to be configured to ensure that operations across the storage and the message broker are atomic. See the documentation on [transaction consistency](/nservicebus/transactional-session/#transaction-consistency) for more details.

partial: transactionalsessionoptions

partial: processorconfiguration

ASP.NET Core uses `ConfigureWebHostDefaults` for configuration and a custom result filter is registered for the `ITransactionalSession` lifetime management:

snippet: txsession-web-configuration

Entity Framework support is configured by registering the `DbContext`:

snippet: txsession-ef-configuration

The registration ensures that the `MyDataContext` type is built using the same session and transaction that is used by the `ITransactionalSession`. Once the transactional session is committed, it notifies the Entity Framework context to call `SaveChangesAsync`. When the transactional session is not used, a data context with a dedicated connection is returned.

## Using the session

The message session is injected into `SendMessageController` via method injection. Message operations executed on the `ITransactionalSession` API are transactionally consistent with the database operations performed on the `MyDataContext`.

snippet: txsession-controller

The lifecycle of the session is managed by the `MessageSessionFilter` which hooks into the [result filter](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-7.0#iresultfilter-and-iasyncresultfilter) of the ASP.NET pipeline. When a controller action with an `ITransactionalSession` parameter is called, the filter opens the session, performs the next action, and then commits the session:

snippet: txsession-filter

> [!NOTE]
> The resource filter could be extended to return problem details (for example, with `context.Result = new ObjectResult(new ProblemDetails())`) in cases where the transactional session cannot be committed. This is omitted from the sample.

For controller actions that do not have `ITransactionalSession` parameter, navigate to `http://localhost:58118/all`, a data context with a dedicated connection is used.

snippet: txsession-controller-query

> [!NOTE]
> The sample uses method injection as an opinionated way of expressing the need for having the transactional boundaries managed by the infrastructure. If it is preferred to express the transactional boundaries with an attribute to make sure even complex dependency chains get access to the transactional session, without needing to inject that into the controller action, an [action attribute](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?#action-filters) must be used to annotate controllers or actions.

For example, navgiate to `http://localhost:58118/service` which injects a service that depends on `ITransactionalSession`.

snippet: txsession-controller-attribute

The `[RequiresTransactionalSession]` attribute makes sure the session is opened and committed.

snippet: txsession-filter-attribute

as long as the attribute is registered in the configuration

snippet: txsession-web-configuration-attribute

This diagram visualizes the interaction between the resource filter, `ITransactionalSession`, and the Web API controller:

```mermaid
sequenceDiagram
    autonumber
    User->>Filter: Http Request
    activate Filter
    Filter->>TransactionalSession: Open
    activate TransactionalSession
    TransactionalSession-->>Filter: Reply
    Filter->>Controller: next()
    activate Controller
    Controller->>TransactionalSession: Send/Publish...
    Controller->>TransactionalSession: Use SynchronizedStorageSession
    deactivate Controller
    Filter->>TransactionalSession: Commit
    deactivate TransactionalSession
    Filter-->>User: Reply
    deactivate Filter
```

## Handling the message

The `MyHandler` handles the message sent by the WebAPI and accesses the previously committed data stored by the controller:

snippet: txsession-handler
