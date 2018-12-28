---
title: Using NServiceBus.Router for atomic update-and-publish in WebAPI
summary: How to use SQL transport at the ASP.NET frontend to allow for atomic update-and-publish and NServiceBus.Router to connect frontend to backend transport
component: Router
reviewed: 2018-12-28
related:
- nservicebus/router
- samples/web/send-from-aspnetcore-webapi
---


This sample shows how to use SQL Server transport in the ASP.NET Core WebAPI application to implement atomic update-and-publish while using a different transport for the backend. It intentionally skips over basic concepts related to hosting NServiceBus endpoints in the MVC application. To learn more about that, see the [basic MVC Core sample](/samples/web/send-from-aspnetcore-webapi).

Very often the web controller needs to modify the data in a database and publish a message as part of handling a single HTTP request. For consistency these two operations need to be done atomically. That means that if either of them fails, nothing is visible to the outside world. If the transport used by the system does not support distributed transactions, the solution is to use a dedicated specialized transport only for the frontend, and route messages between the two transports using NServiceBus.Router. 

The most convenient transport for the frontend is SQL Server transport as it can easily share the connection and transaction with data access library such as EntityFramework.


## Running the solution

When the solution is run, four console windows are open. Press `enter` in the Client window to make a HTTP call to the Frontend. The HTTP request is handled by an async [WebAPI](https://www.asp.net/web-api) controller. It creates an `Order` entity through EntityFramework and publishes an NServiceBus even. The event is router, via the Router, to the Backend application which logs the fact that it received the message. 


## Prerequisites

include: sql-prereq

This sample automatically creates database `update_and_publish`.


## Code walk-through

The solution consists of five projects.


### Shared

The Shared project contains the message contracts.


### Client

The Client project calls the Frontend HTTP API to create orders. It does not use NServiceBus.


### Frontend

The Frontend project contains an NServiceBus endpoint that runs the SQL Server transport.

snippet: EndpointConfiguration

In contains an async controller for handling API requests that stores data in a database and publishes an event

snippet: MessageSessionUsage

NOTE: Both connection and transaction objects need to be passed to EntityFramework session. Connection and transaction are passed to NServiceBus transport using the `TransportTransaction` API.


### Backend

The Backend project contains an NServiceBus endpoint that runs the Learning transport. It processes `OrderAccepted` events. Because these events are published by an endpoint that is hosted behind a router, NServiceBus.Router subscription API has to be used

snippet: Routing

NOTE: The publisher (Frontend endpoint) does not need any routing configuration.

In real-life systems the backend is likely to not be monolithic (single endpoint) but rather a collection of endpoints communicating via NServiceBus. Since this sample assumes that the backend transport does not support distributed transactions, in order to provide **effectively-once** message processing guarantees in the backend endpoints the [Outbox](/nservicebus/outbox) feature should be used.

### Router

The Router project sets up a bi-directional connection between SQL Server and Learning transports.

snippet: RouterConfig