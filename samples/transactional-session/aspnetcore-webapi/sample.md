---
title: Using TransactionalSession with Entity Framework and ASP.NET Core
summary: NServiceBus.TransactionalSession sample that illustrates how to send messages and modify data with Entity Framework atomically in ASP.NET Core.
component: TransactionalSession
reviewed: 2022-08-12
related:
  - TODO
---

include: webhost-warning

This sample shows how to send messages and modify data in a database atomically using the `NServiceBus.TransactionalSession` package. The operations are triggered by an incoming HTTP request to ASP.NET which will manage the `ITransactionalSession` lifetime.

## Prerequisites

- Visual Studio 2019 is required to run this sample.
- LocalDB support is required to run the sample. A custom connection string needs to be configured otherwise.

## Running the solution

When the solution is run, a new browser window/tab opens, as well as a console application. The browser will navigate to `http://localhost:58118/`.

An async [WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) controller handles the request. It stores a new document using Entity Framework and sends an NServiceBus message to the endpoint running in the console application. 

The message will be processed by the NServiceBus message handler and print `"Message received at endpoint"` to the console. The message handler also updates the previously created entity.

## Configuration

The host is configures NServiceBus with the `UseNServiceBus` extension method:

snippet: txsession-nsb-configuration

The configuration enables the transactional session functionalty using `endpointConfiguration.EnableTransactionalSession()`. Note that the transactional session feature also requires the outbox to be enabled.

ASP.NET Core is configured using the `ConfigureWebHostDefaults` extension method. A custom middleware is registered to manage the `ITransactionalSession` lifetime:

snippet: txsession-web-configuration

Entity Framework support is configured by this registration for the `DbContext`:

snippet: txsession-ef-configuration

This registration registers the `MyDataContext` type to be built using the same session and transaction used by the `ITransactionalSession`. Once the transactional session is committed, it notifies the Entity Framework context to call `SaveChangesAsync`.

## Using the session

The message session is injected into `SendMessageController` via constructor injection along with the entity framework database context. Message operations executed on the `ITransactionalSession` API are transactionally consistent with the database operations performed on the `MyDataContext`.

snippet: txsession-controller

The session has already been opened by the `MessageSessionMiddleware`. The middleware will also take care of committing the session once the ASP.NET pipeline completed:

snippet: txsession-middleware

## Handling the message

The `MyHandler` message handler handles the message sent by the ASP.NET controller. It can access the previously committed data stored by the controller:

snippet: txsession-handler

The message session is injected into `SendMessageController` via constructor injection.

snippet: MessageSessionInjection